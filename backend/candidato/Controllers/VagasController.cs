using Microsoft.AspNetCore.Mvc;
using candidatos.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AutoMapper;
using candidato.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace candidato.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VagasController : ControllerBase
{
    private readonly IFachadaVaga _fachadaVaga;
    private readonly IMapper _mapper;

    public VagasController(IFachadaVaga fachadaVaga, IMapper mapper)
    {
        _fachadaVaga = fachadaVaga;
        _mapper = mapper;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetVagasAbertas()
    {
        var vagas = await _fachadaVaga.ObterAbertas();
        var vagasDto = _mapper.Map<List<VagaDto>>(vagas);
        return Ok(vagasDto);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetVaga(long id)
    {
        var vaga = await _fachadaVaga.ObterDetalhes(id);
        if (vaga == null) return NotFound();
        var vagaDto = _mapper.Map<VagaDto>(vaga);
        return Ok(vagaDto);
    }

    [HttpPost]
    [Authorize(Roles = "Recrutador")]
    public async Task<IActionResult> CreateVaga([FromBody] VagaDto dto)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "nameid")?.Value;
        if (userIdStr == null) return Unauthorized("Usuário não autenticado (Claim não encontrada)");
        
        var userId = long.Parse(userIdStr);
        var vagaModel = _mapper.Map<Vaga>(dto);
        var vaga = await _fachadaVaga.CriarVaga(vagaModel, userId);

        var vagaRetornoDto = _mapper.Map<VagaDto>(vaga);
        return Created($"/api/vagas/{vaga.Id}", vagaRetornoDto);
    }
    
    [HttpGet("minhas")]
    [Authorize(Roles = "Recrutador")]
    public async Task<IActionResult> GetMinhasVagas()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "nameid")?.Value;
        if (userIdStr == null) return Unauthorized("Usuário não autenticado (Claim não encontrada)");

        var userId = long.Parse(userIdStr);
        var vagas = await _fachadaVaga.ObterMinhasVagas(userId);
        var vagasDto = _mapper.Map<List<VagaDto>>(vagas);
        return Ok(vagasDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Recrutador,Admin")]
    public async Task<IActionResult> DeleteVaga(long id)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "nameid")?.Value;
        if (userIdStr == null) return Unauthorized("Usuário não autenticado");

        var userId = long.Parse(userIdStr);
        var success = await _fachadaVaga.RemoverVaga(id, userId);

        if (!success)
            return Forbid("Você não tem permissão para remover esta vaga ou ela não existe.");

        return NoContent();
    }
}
