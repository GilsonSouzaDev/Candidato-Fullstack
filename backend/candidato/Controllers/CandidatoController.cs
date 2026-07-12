using candidato.Controllers.Fachadas;
using candidato.DTOs;
using candidatos.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using AutoMapper;

namespace candidato.Controllers;


[EnableCors("AnotherPolicy")]
[Route("api/[controller]")]
[ApiController]
public class CandidatoController : ControllerBase
{

    private readonly IFachadaCandidato _fachadaCandidato;
    private readonly IMapper _mapper;

    public CandidatoController(IFachadaCandidato fachadaCandidato, IMapper mapper)
    {
        _fachadaCandidato = fachadaCandidato;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<CandidatoDto>>> ExibirCandidatos()
    {
        List<Candidato> candidatos = await _fachadaCandidato.ObterTodos();
        var candidatosDto = _mapper.Map<List<CandidatoDto>>(candidatos);
        return Ok(candidatosDto);
    }

    [HttpGet("display/{id}")]
    public async Task<ActionResult<CandidatoDto>> BuscarPorID(long id)
    {
        Candidato candidato = await _fachadaCandidato.ObterPorId(id);
        var candidatoDto = _mapper.Map<CandidatoDto>(candidato);
        return Ok(candidatoDto);
    }

    [HttpGet("meu-perfil")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Candidato")]
    public async Task<ActionResult<CandidatoDto>> ObterMeuPerfil()
    {
        var userIdStr = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdStr == null) return Unauthorized();
        var userId = long.Parse(userIdStr);

        var candidatos = await _fachadaCandidato.ObterTodos();
        var meuPerfil = candidatos.FirstOrDefault(c => c.UsuarioId == userId);
        
        if (meuPerfil == null) return NotFound("Perfil não encontrado");
        
        var meuPerfilDto = _mapper.Map<CandidatoDto>(meuPerfil);
        return Ok(meuPerfilDto);
    }

    [HttpPost]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Candidato")]
    public async Task<ActionResult<CandidatoDto>> CadastrarCandidato([FromBody] CandidatoDto candidatoDtoModel)
    {
        var userIdStr = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdStr == null) return Unauthorized();
        
        candidatoDtoModel.UsuarioId = long.Parse(userIdStr);
        
        var candidatoModel = _mapper.Map<Candidato>(candidatoDtoModel);
        Candidato candidato = await _fachadaCandidato.Adicionar(candidatoModel);
        
        var candidatoRetornoDto = _mapper.Map<CandidatoDto>(candidato);
        return Ok(candidatoRetornoDto);
    }

    [HttpPut("edit/{id}")]
    public async Task<ActionResult<CandidatoDto>> AtualizarCandidato(long id, [FromBody] CandidatoDto candidatoDtoModel)
    {
        candidatoDtoModel.Id = id;
        
        var candidatoModel = _mapper.Map<Candidato>(candidatoDtoModel);
        Candidato candidato = await _fachadaCandidato.Atualizar(id, candidatoModel);
        
        var candidatoRetornoDto = _mapper.Map<CandidatoDto>(candidato);
        return Ok(candidatoRetornoDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeletarCandidato(long id)
    {
        bool apagado = await _fachadaCandidato.Remover(id);
        return Ok(apagado);
    }
}