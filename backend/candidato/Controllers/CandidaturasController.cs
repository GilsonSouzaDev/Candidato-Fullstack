using Microsoft.AspNetCore.Mvc;
using candidatos.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using candidato.DTOs;

namespace candidato.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CandidaturasController : ControllerBase
{
    private readonly IFachadaCandidatura _fachadaCandidatura;

    public CandidaturasController(IFachadaCandidatura fachadaCandidatura)
    {
        _fachadaCandidatura = fachadaCandidatura;
    }

    [HttpPost("aplicar")]
    [Authorize(Roles = "Candidato")]
    public async Task<IActionResult> Aplicar([FromBody] CandidaturaDto dto)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr == null) return Unauthorized();
        var userId = long.Parse(userIdStr);

        try
        {
            await _fachadaCandidatura.Aplicar(dto, userId);
            return Ok(new { Message = "Candidatura enviada com sucesso!" });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("minhas")]
    [Authorize(Roles = "Candidato")]
    public async Task<IActionResult> GetMinhasInscricoes()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr == null) return Unauthorized();
        var userId = long.Parse(userIdStr);

        var inscricoes = await _fachadaCandidatura.ObterMinhasInscricoes(userId);
        return Ok(inscricoes);
    }

    [HttpGet("vaga/{vagaId}")]
    [Authorize]
    public async Task<IActionResult> GetCandidaturasDaVaga(long vagaId)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr == null) return Unauthorized();
        var userId = long.Parse(userIdStr);

        try
        {
            var candidaturas = await _fachadaCandidatura.ObterCandidaturasDaVaga(vagaId, userId);
            return Ok(candidaturas);
        }
        catch (System.Exception ex) when (ex.Message == "NOT_FOUND")
        {
            return NotFound(new { Error = "Vaga não encontrada" });
        }
        catch (System.Exception ex) when (ex.Message == "FORBIDDEN")
        {
            return StatusCode(403, new { Error = "Acesso negado. Apenas o recrutador criador da vaga pode ver os candidatos." });
        }
    }

    [HttpPut("{id}/status")]
    [Authorize]
    public async Task<IActionResult> AtualizarStatus(long id, [FromBody] UpdateStatusDto dto)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr == null) return Unauthorized();
        var userId = long.Parse(userIdStr);

        try
        {
            await _fachadaCandidatura.AtualizarStatus(id, dto.Status, userId);
            return Ok(new { Message = "Status atualizado com sucesso" });
        }
        catch (System.Exception ex) when (ex.Message == "NOT_FOUND")
        {
            return NotFound(new { Error = "Candidatura não encontrada" });
        }
        catch (System.Exception ex) when (ex.Message == "FORBIDDEN")
        {
            return StatusCode(403, new { Error = "Acesso negado." });
        }
    }

    [HttpDelete("vaga/{vagaId}")]
    [Authorize(Roles = "Candidato")]
    public async Task<IActionResult> Cancelar(long vagaId)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr == null) return Unauthorized();
        var userId = long.Parse(userIdStr);

        try
        {
            await _fachadaCandidatura.Cancelar(vagaId, userId);
            return Ok(new { Message = "Inscrição cancelada com sucesso!" });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
