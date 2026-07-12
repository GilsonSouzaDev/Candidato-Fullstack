using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using candidato.DataAccess;
using candidatos.Models;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;

namespace candidato.Controllers.Fachadas
{
    public class FachadaAuth : IFachadaAuth
    {
        private readonly IUsuarioDao _usuarioDao;
        private readonly IConfiguration _configuration;

        public FachadaAuth(IUsuarioDao usuarioDao, IConfiguration configuration)
        {
            _usuarioDao = usuarioDao;
            _configuration = configuration;
        }

        public async Task<Usuario> Registrar(RegisterDto dto)
        {
            if (await _usuarioDao.ObterPorEmail(dto.Email) != null)
                throw new Exception("Usuário já existe");

            var user = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                Role = "Candidato"
            };

            return await _usuarioDao.Adicionar(user);
        }

        public async Task<Usuario> RegistrarRecrutador(RegisterDto dto)
        {
            if (await _usuarioDao.ObterPorEmail(dto.Email) != null)
                throw new Exception("Usuário já existe");

            var user = new Usuario
            {
                Nome = string.IsNullOrWhiteSpace(dto.Nome) ? "Recrutador Padrão" : dto.Nome,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                Role = "Recrutador"
            };

            return await _usuarioDao.Adicionar(user);
        }

        public async Task<string> Login(LoginDto dto)
        {
            var user = await _usuarioDao.ObterPorEmail(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, user.SenhaHash))
                throw new Exception("Credenciais inválidas");

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = _configuration["Jwt:Key"] ?? "GilsonPortfolioSuperSecretKey2026!@#";
            var key = Encoding.UTF8.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
