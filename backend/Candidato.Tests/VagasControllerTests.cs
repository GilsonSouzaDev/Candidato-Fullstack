using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Candidato.Tests
{
    public class VagasControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public VagasControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        private async Task<string> GetAuthTokenAsync(string email, string password)
        {
            var loginData = new { Email = email, Senha = password };
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginData);
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            return result.GetProperty("token").GetString();
        }

        [Fact]
        public async Task GetVagasAbertas_ReturnsList()
        {
            var response = await _client.GetAsync("/api/vagas");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task CreateVaga_AsRecrutador_ReturnsCreated()
        {
            string token = await GetAuthTokenAsync("recrutador@sistema.com", "recrutador123");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var novaVaga = new
            {
                Titulo = "Vaga Teste Automatizado",
                Descricao = "Descrição da vaga de teste automatizado.",
                Requisitos = "Saber programar",
                Salario = 5000.0m
            };

            var response = await _client.PostAsJsonAsync("/api/vagas", novaVaga);

            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.Created, because: content);
        }

        [Fact]
        public async Task CreateVaga_AsCandidato_ReturnsForbidden()
        {
            var registerData = new
            {
                Email = "candidatoforbidden@sistema.com",
                Senha = "senhaforte123",
                Nome = "Candidato Sem Acesso"
            };
            
            var regResponse = await _client.PostAsJsonAsync("/api/auth/register", registerData);
            regResponse.IsSuccessStatusCode.Should().BeTrue();

            string token = await GetAuthTokenAsync("candidatoforbidden@sistema.com", "senhaforte123");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var novaVaga = new
            {
                Titulo = "Vaga Não Deve Passar",
                Descricao = "Teste"
            };

            var response = await _client.PostAsJsonAsync("/api/vagas", novaVaga);
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
