using System;
using System.Collections.Generic;
using System.Linq;
using candidato.Data;
using candidatos.Data;
using candidatos.Models;
using Microsoft.EntityFrameworkCore;



namespace candidato.DataAccess.daos;

public class CandidatoDao: IDao
{
    private readonly CandidatoContext _dbContext;

    public CandidatoDao(CandidatoContext context)
    {
        _dbContext = context;
    }


    public async Task<Candidato> Adicionar(Candidato entidade)
    {
        await _dbContext.AddAsync(entidade);
        await _dbContext.SaveChangesAsync();

        return entidade;
    }


    public async Task<Candidato> Atualizar(Candidato entidade)
    {
        Candidato candidatoPorId = await ObterPorId(entidade.Id);
       
        if (candidatoPorId == null)
        {
            throw new Exception($"Usuario para o ID: {entidade.Id} não foi encontrado no banco de dados");
        }
  
        candidatoPorId.Nome = entidade.Nome;
        candidatoPorId.Filiacao = entidade.Filiacao;
        candidatoPorId.Endereco = entidade.Endereco;
        candidatoPorId.Telefones = entidade.Telefones;
        candidatoPorId.Cursos = entidade.Cursos;
        
        _dbContext.Candidatos.Update(candidatoPorId);
        _dbContext.SaveChanges();

        return candidatoPorId;
        
    }


    public async Task<Candidato> ObterPorId(long id)
    {
        return await _dbContext.Candidatos.AsNoTracking()
            .Include(c => c.Telefones)
            .Include(c => c.Cursos)
            .Include(c => c.Endereco)
                .ThenInclude(e => e.Cidade)
                .ThenInclude(c => c.Estado)
            .Include(c => c.Filiacao)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Candidato>> ObterTodos()
    {
        return await _dbContext.Candidatos
            .Include(c => c.Telefones)
            .Include(c => c.Cursos)
            .Include(c => c.Endereco)
                .ThenInclude(e => e.Cidade)
                .ThenInclude(c => c.Estado)
            .Include(c => c.Filiacao)
            .ToListAsync();
    }

    public async Task<bool> Remover(long id)
    {

        var candidato = await _dbContext.Candidatos
            
            .Include(c => c.Telefones)
            .Include(c => c.Cursos)
            .Include(c => c.Endereco)
                .ThenInclude(e => e.Cidade)
                    .ThenInclude(c => c.Estado)
            .Include(c => c.Filiacao)
        
            .FirstOrDefaultAsync(x => x.Id == id);
        if (candidato == null)
        {
            throw new Exception($"Candidato com o ID: {id} não foi encontrado no banco de dados");
        }
        
      
        _dbContext.Telefones.RemoveRange(candidato.Telefones);
        _dbContext.Cursos.RemoveRange(candidato.Cursos);
        // Remover Endereco, Cidade e Estado
        _dbContext.Enderecos.Remove(candidato.Endereco);
        _dbContext.Cidades.Remove(candidato.Endereco.Cidade);
        _dbContext.Estados.Remove(candidato.Endereco.Cidade.Estado);

        _dbContext.Filiacoes.Remove(candidato.Filiacao);
        
        _dbContext.Candidatos.Remove(candidato);
        await _dbContext.SaveChangesAsync();

        return true;
    }

  
}