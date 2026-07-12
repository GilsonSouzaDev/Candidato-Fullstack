using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Candidato.Tests
{
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_WithValidAdmin_ReturnsToken()
        {
            var loginData = new
            {
                Email = "admin@sistema.com",
                Senha = "admin123"
            };

            var response = await _client.PostAsJsonAsync("/api/auth/login", loginData);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            string token = result.GetProperty("token").GetString();
            token.Should().NotBeNullOrEmpty();
            // AuthController.Login only returns "token", role is inside the JWT claims
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            var loginData = new
            {
                Email = "admin@sistema.com",
                Senha = "wrongpassword"
            };

            var response = await _client.PostAsJsonAsync("/api/auth/login", loginData);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task RegisterCandidato_WithValidData_ReturnsSuccess()
        {
            var registerData = new
            {
                Email = "teste_novo_candidato@sistema.com",
                Senha = "senhaforte123",
                Nome = "Novo Candidato Teste"
            };

            var response = await _client.PostAsJsonAsync("/api/auth/register", registerData);
            
            var content = await response.Content.ReadAsStringAsync();
            response.IsSuccessStatusCode.Should().BeTrue(because: content);
        }
    }
}
