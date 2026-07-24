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

app.UseCors(MyAllowSpecificOrigins);

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

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
    if (!db.Usuarios.Any(u => u.Email == "candidato_extra1@candidato.com"))
    {
        // 1. Add extra candidates
        var cand1User = new Usuario { Nome = "João Silva", Email = "candidato_extra1@candidato.com", SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"), Role = "Candidato" };
        var cand2User = new Usuario { Nome = "Maria Oliveira", Email = "candidato_extra2@candidato.com", SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"), Role = "Candidato" };
        var cand3User = new Usuario { Nome = "Pedro Santos", Email = "candidato_extra3@candidato.com", SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"), Role = "Candidato" };
        db.Usuarios.AddRange(cand1User, cand2User, cand3User);
        db.SaveChanges();

        var cand1 = new Candidato { Nome = "João Silva", Cpf = "33333333333", DataNascimento = new DateTime(1990, 2, 20).ToUniversalTime(), ResumoProfissional = "Desenvolvedor Backend Pleno com experiência em C# e Java.", LinkedInUrl = "https://linkedin.com/in/joaosilva", UsuarioId = cand1User.Id, Endereco = new Endereco { Logradouro = "Rua A", Numero = "10", Cep = "01000-000", Bairro = "Centro", Cidade = new Cidade { Nome = "São Paulo", Estado = new Estado { Nome = "São Paulo", Sigla = "SP" } } }, Filiacao = new Filiacao { NomeMae = "Mãe do João", NomePai = "Pai do João" } };
        var cand2 = new Candidato { Nome = "Maria Oliveira", Cpf = "44444444444", DataNascimento = new DateTime(1992, 7, 15).ToUniversalTime(), ResumoProfissional = "Especialista em Frontend e UX Design.", LinkedInUrl = "https://linkedin.com/in/mariaoliveira", UsuarioId = cand2User.Id, Endereco = new Endereco { Logradouro = "Rua B", Numero = "20", Cep = "02000-000", Bairro = "Jardins", Cidade = new Cidade { Nome = "São Paulo", Estado = new Estado { Nome = "São Paulo", Sigla = "SP" } } }, Filiacao = new Filiacao { NomeMae = "Mãe da Maria", NomePai = "Pai da Maria" } };
        var cand3 = new Candidato { Nome = "Pedro Santos", Cpf = "55555555555", DataNascimento = new DateTime(1995, 11, 30).ToUniversalTime(), ResumoProfissional = "Engenheiro de Dados Sênior.", LinkedInUrl = "https://linkedin.com/in/pedrosantos", UsuarioId = cand3User.Id, Endereco = new Endereco { Logradouro = "Rua C", Numero = "30", Cep = "03000-000", Bairro = "Botafogo", Cidade = new Cidade { Nome = "Rio de Janeiro", Estado = new Estado { Nome = "Rio de Janeiro", Sigla = "RJ" } } }, Filiacao = new Filiacao { NomeMae = "Mãe do Pedro", NomePai = "Pai do Pedro" } };
        db.Candidatos.AddRange(cand1, cand2, cand3);
        db.SaveChanges();

        // 2. Add extra jobs
        var v1 = new Vaga { Titulo = "Desenvolvedor Backend .NET", Descricao = "Oportunidade para atuar com microserviços e .NET 8.", Requisitos = "C#, .NET Core, SQL Server", NomeEmpresa = "Tech Solutions", Beneficios = "VR, Plano de Saúde", Atividades = "Desenvolvimento de APIs", RequisitosObrigatorios = "C# Avançado", RequisitosDesejaveis = "Docker, Kubernetes", Salario = 9500.00m, StatusAberta = true, DataCriacao = DateTime.UtcNow, CriadoPorId = recrutador.Id };
        var v2 = new Vaga { Titulo = "Engenheiro de Dados", Descricao = "Vaga para atuar na estruturação do Data Lake.", Requisitos = "Python, SQL, AWS", NomeEmpresa = "Data Corp", Beneficios = "VA, Gympass", Atividades = "ETL, modelagem de dados", RequisitosObrigatorios = "Python, SQL", RequisitosDesejaveis = "Spark, Airflow", Salario = 12000.00m, StatusAberta = true, DataCriacao = DateTime.UtcNow.AddDays(-5), CriadoPorId = recrutador.Id };
        var v3 = new Vaga { Titulo = "DevOps Engineer", Descricao = "Buscamos um profissional para automatizar nossas esteiras CI/CD.", Requisitos = "Linux, CI/CD, AWS", NomeEmpresa = "CloudOps", Beneficios = "VR, Home Office", Atividades = "Gestão de infraestrutura", RequisitosObrigatorios = "Terraform, Ansible", RequisitosDesejaveis = "Certificação AWS", Salario = 11000.00m, StatusAberta = true, DataCriacao = DateTime.UtcNow.AddDays(-10), CriadoPorId = recrutador.Id };
        db.Vagas.AddRange(v1, v2, v3);
        db.SaveChanges();

        // 3. Add candidaturas (applications)
        var app1 = new Candidatura { CandidatoId = cand1.Id, VagaId = v1.Id, StatusCandidatura = "Triagem", DataAplicacao = DateTime.UtcNow.AddDays(-2) };
        var app2 = new Candidatura { CandidatoId = cand1.Id, VagaId = v2.Id, StatusCandidatura = "Entrevista", DataAplicacao = DateTime.UtcNow.AddDays(-4) };
        var app3 = new Candidatura { CandidatoId = cand2.Id, VagaId = v3.Id, StatusCandidatura = "Contratado", DataAplicacao = DateTime.UtcNow.AddDays(-8) };
        var app4 = new Candidatura { CandidatoId = cand3.Id, VagaId = v2.Id, StatusCandidatura = "Teste Técnico", DataAplicacao = DateTime.UtcNow.AddDays(-1) };
        
        var vagaExistente = db.Vagas.FirstOrDefault(v => v.Titulo.Contains("Angular"));
        if (vagaExistente != null)
        {
            var app5 = new Candidatura { CandidatoId = cand2.Id, VagaId = vagaExistente.Id, StatusCandidatura = "Triagem", DataAplicacao = DateTime.UtcNow };
            db.Candidaturas.Add(app5);
        }

        db.Candidaturas.AddRange(app1, app2, app3, app4);
        db.SaveChanges();
    }
}

app.Run();
