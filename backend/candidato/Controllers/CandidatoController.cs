
using candidato.Controllers.Fachadas;
using candidato.Data;
using candidato.DataAccess;
using candidato.DataAccess.daos;
using candidatos.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace candidato.Controllers;


[EnableCors("AnotherPolicy")]
[Route("api/[controller]")]
[ApiController]
public class CandidatoController : ControllerBase
{

    private readonly IFachadaCandidato _fachadaCandidato;

    public CandidatoController(IFachadaCandidato fachadaCandidato)
    {
        _fachadaCandidato = fachadaCandidato;

    }

    [HttpGet]
    public async Task<ActionResult<List<Candidato>>> ExibirCandidatos()
    {
        List<Candidato> candidatos = await _fachadaCandidato.ObterTodos();

        return Ok(candidatos);
    }

    [HttpGet("display/{id}")]
    public async Task<ActionResult<Candidato>> BuscarPorID(long id)
    {
        Candidato candidato = await _fachadaCandidato.ObterPorId(id);
        return Ok(candidato);
    }

    [HttpPost]
    public async Task<ActionResult<Candidato>> CadastrarCandidato([FromBody] Candidato candidatoModel)
    {
        Candidato candidato = await _fachadaCandidato.Adicionar(candidatoModel);
        return Ok(candidato);
    }

    [HttpPut("edit/{id}")]
    public async Task<ActionResult<Candidato>> AtualizarCandidato(long id,[FromBody] Candidato candidatoModel)
    {
        candidatoModel.Id = id;
        Candidato candidato = await _fachadaCandidato.Atualizar(id, candidatoModel);
        return Ok(candidato);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Candidato>> DeletarCandidato(long id)
    {
        bool apagado = await _fachadaCandidato.Remover(id);
        return Ok(apagado);
    }
}