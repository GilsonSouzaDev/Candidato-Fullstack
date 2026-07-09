// old strategies removed
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


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CandidatoContext>(opts => {
    opts.UseSqlite("Data Source=temp-ats.db");
});


// Add services to the container.
builder.Services.AddControllers();

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

// Authentication JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "GilsonPortfolioSuperSecretKey2026!@#";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CandidatoContext>();
    db.Database.EnsureCreated();
}

app.Run();
