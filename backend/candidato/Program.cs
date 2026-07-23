// old strategies removed
using System.Text.Json.Serialization;
using FluentValidation;
using candidato.Validations;
using candidato.Middlewares;
using candidato.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using candidato.Controllers.Fachadas;
using candidato.DataAccess;
using candidato.DataAccess.daos;
using candidatos.Data;
using candidatos.Models;
using Microsoft.EntityFrameworkCore;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                      });
});

//Definindo o acesso ao Banco de dados


var connectionString = builder.Configuration.GetConnectionString("OracleConnection");
builder.Services.AddDbContext<CandidatoContext>(opts => {
    opts.UseOracle(connectionString, b => b.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19));
});


// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options => 
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar FluentValidation no contêiner
builder.Services.AddValidatorsFromAssemblyContaining<CandidatoValidator>();

// Registrar injeções de dependencia no contêiner
builder.Services.AddScoped<IFachadaCandidato, FachadaCandidato>();
builder.Services.AddScoped<IDao, CandidatoDao>();

builder.Services.AddScoped<IUsuarioDao, UsuarioDao>();
builder.Services.AddScoped<IFachadaAuth, FachadaAuth>();

builder.Services.AddScoped<IVagaDao, VagaDao>();
builder.Services.AddScoped<IFachadaVaga, FachadaVaga>();

builder.Services.AddScoped<ICandidaturaDao, CandidaturaDao>();
builder.Services.AddScoped<IFachadaCandidatura, FachadaCandidatura>();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Authentication JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "GilsonPortfolioSuperSecretKey2026!@#";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CandidatoContext>();
    db.Database.Migrate();

    if (!db.Usuarios.Any(u => u.Role == "Admin"))
    {
        db.Usuarios.Add(new Usuario
        {
            Nome = "Administrador Geral",
            Email = "admin@candidato.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            Role = "Admin"
        });
        db.SaveChanges();
    }

    var recrutador = db.Usuarios.FirstOrDefault(u => u.Role == "Recrutador");
    if (recrutador == null)
    {
        recrutador = new Usuario
        {
            Nome = "Recrutador Seed",
            Email = "recrutador@sistema.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("recrutador123"),
            Role = "Recrutador"
        };
        db.Usuarios.Add(recrutador);
        db.SaveChanges();
    }

    if (!db.Vagas.Any())
    {
        db.Vagas.Add(new Vaga
        {
            Titulo = "Desenvolvedor Full Stack Angular + .NET",
            Descricao = "Vaga para atuar no desenvolvimento de um ATS Premium. Necessário conhecimento em Angular e .NET.",
            Requisitos = "Angular, .NET, C#",
            NomeEmpresa = "Empresa ATS",
            Beneficios = "VR, VA, Plano de Saúde",
            Atividades = "Desenvolvimento de novas features",
            RequisitosObrigatorios = "Angular, .NET",
            RequisitosDesejaveis = "Docker, Oracle",
            Salario = 8000.00m,
            StatusAberta = true,
            DataCriacao = DateTime.UtcNow,
            CriadoPorId = recrutador.Id
        });
        
        db.Vagas.Add(new Vaga
        {
            Titulo = "UX/UI Designer Sênior",
            Descricao = "Buscamos um designer com forte experiência em Glassmorphism e interfaces limpas.",
            Requisitos = "Figma, Design System",
            NomeEmpresa = "Empresa ATS",
            Beneficios = "VR, VA, Plano de Saúde",
            Atividades = "Prototipagem de telas",
            RequisitosObrigatorios = "Figma",
            RequisitosDesejaveis = "UX Research",
            Salario = 7500.00m,
            StatusAberta = true,
            DataCriacao = DateTime.UtcNow.AddDays(-2),
            CriadoPorId = recrutador.Id
        });
        
        db.SaveChanges();
    }

    if (!db.Usuarios.Any(u => u.Email == "candidato@candidato.com"))
    {
        var candidato = new Usuario
        {
            Nome = "Candidato Seed",
            Email = "candidato@candidato.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            Role = "Candidato"
        };
        db.Usuarios.Add(candidato);
        db.SaveChanges();
        
        var curriculumCandidato = new Candidato
        {
            Nome = "Candidato Seed",
            Cpf = "22222222222",
            DataNascimento = new DateTime(1998, 5, 10).ToUniversalTime(),
            ResumoProfissional = "Desenvolvedor de Software Iniciante",
            LinkedInUrl = "https://linkedin.com/in/candidatoseed",
            UsuarioId = candidato.Id,
            Endereco = new Endereco
            {
                Logradouro = "Rua Teste",
                Numero = "456",
                Cep = "00000-000",
                Bairro = "Centro",
                Cidade = new Cidade
                {
                    Nome = "São Paulo",
                    Estado = new Estado
                    {
                        Nome = "São Paulo",
                        Sigla = "SP"
                    }
                }
            },
            Filiacao = new Filiacao
            {
                NomeMae = "Mãe do Candidato Seed",
                NomePai = "Pai do Candidato Seed"
            },
            Telefones = new List<Telefone>
            {
                new Telefone { Numero = "11999998888", Tipo = TipoTelefone.Celular }
            }
        };
        db.Candidatos.Add(curriculumCandidato);
        db.SaveChanges();
    }

    if (!db.Usuarios.Any(u => u.Email == "candidato_vagas@candidato.com"))
    {
        var candidatoVagas = new Usuario
        {
            Nome = "Candidato Vagas",
            Email = "candidato_vagas@candidato.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            Role = "Candidato"
        };
        db.Usuarios.Add(candidatoVagas);
        db.SaveChanges();
        
        var curriculum = new Candidato
        {
            Nome = "Candidato Vagas",
            Cpf = "11111111111",
            DataNascimento = new DateTime(1995, 1, 1).ToUniversalTime(),
            ResumoProfissional = "Desenvolvedor de Software Especialista",
            LinkedInUrl = "https://linkedin.com/in/candidatovagas",
            UsuarioId = candidatoVagas.Id,
            Endereco = new Endereco
            {
                Logradouro = "Rua Teste",
                Numero = "123",
                Cep = "00000-000",
                Bairro = "Centro",
                Cidade = new Cidade
                {
                    Nome = "São Paulo",
                    Estado = new Estado
                    {
                        Nome = "São Paulo",
                        Sigla = "SP"
                    }
                }
            },
            Filiacao = new Filiacao
            {
                NomeMae = "Mãe do Candidato",
                NomePai = "Pai do Candidato"
            }
        };
        db.Candidatos.Add(curriculum);
        db.SaveChanges();
    }
}

app.Run();
