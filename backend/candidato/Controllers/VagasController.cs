using Microsoft.AspNetCore.Mvc;
using candidatos.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace candidato.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VagasController : ControllerBase
{
    private readonly IFachadaVaga _fachadaVaga;

    public VagasController(IFachadaVaga fachadaVaga)
    {
        _fachadaVaga = fachadaVaga;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetVagasAbertas()
    {
        var vagas = await _fachadaVaga.ObterAbertas();
        return Ok(vagas);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetVaga(long id)
    {
        var vaga = await _fachadaVaga.ObterDetalhes(id);
        if (vaga == null) return NotFound();
        return Ok(vaga);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateVaga([FromBody] Vaga dto)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr == null) return Unauthorized();
        
        var userId = long.Parse(userIdStr);
        var vaga = await _fachadaVaga.CriarVaga(dto, userId);

        return Created($"/api/vagas/{vaga.Id}", vaga);
    }
    
    [HttpGet("minhas")]
    [Authorize]
    public async Task<IActionResult> GetMinhasVagas()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr == null) return Unauthorized();

        var userId = long.Parse(userIdStr);
        var vagas = await _fachadaVaga.ObterMinhasVagas(userId);
        return Ok(vagas);
    }
}
