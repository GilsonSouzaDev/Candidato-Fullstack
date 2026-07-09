using Microsoft.AspNetCore.Mvc;
using candidatos.Models;

namespace candidato.Controllers;

public class RegisterDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IFachadaAuth _fachadaAuth;

    public AuthController(IFachadaAuth fachadaAuth)
    {
        _fachadaAuth = fachadaAuth;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            await _fachadaAuth.Registrar(dto);
            return Ok(new { Message = "Usuário registrado com sucesso" });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var token = await _fachadaAuth.Login(dto);
            return Ok(new { Token = token });
        }
        catch (System.Exception ex)
        {
            return Unauthorized(new { Error = ex.Message });
        }
    }
}
