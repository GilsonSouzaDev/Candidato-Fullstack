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
        Candidato candidatoTracked = await _dbContext.Candidatos
            .Include(c => c.Telefones)
            .Include(c => c.Cursos)
            .Include(c => c.Formacoes)
            .Include(c => c.Habilidades)
            .Include(c => c.Experiencias)
            .Include(c => c.Endereco)
                .ThenInclude(e => e.Cidade)
                .ThenInclude(c => c.Estado)
            .Include(c => c.Filiacao)
            .FirstOrDefaultAsync(x => x.Id == entidade.Id);
       
        if (candidatoTracked == null)
        {
            throw new Exception($"Usuario para o ID: {entidade.Id} não foi encontrado no banco de dados");
        }

        // Remove old collections to avoid duplicates
        if (candidatoTracked.Telefones != null) _dbContext.Telefones.RemoveRange(candidatoTracked.Telefones);
        if (candidatoTracked.Cursos != null) _dbContext.Cursos.RemoveRange(candidatoTracked.Cursos);
        if (candidatoTracked.Formacoes != null) _dbContext.Formacoes.RemoveRange(candidatoTracked.Formacoes);
        if (candidatoTracked.Habilidades != null) _dbContext.Habilidades.RemoveRange(candidatoTracked.Habilidades);
        if (candidatoTracked.Experiencias != null) _dbContext.Experiencias.RemoveRange(candidatoTracked.Experiencias);
        
        if (candidatoTracked.Endereco != null)
        {
            _dbContext.Enderecos.Remove(candidatoTracked.Endereco);
            if (candidatoTracked.Endereco.Cidade != null)
            {
                _dbContext.Cidades.Remove(candidatoTracked.Endereco.Cidade);
                if (candidatoTracked.Endereco.Cidade.Estado != null)
                {
                    _dbContext.Estados.Remove(candidatoTracked.Endereco.Cidade.Estado);
                }
            }
        }
        
        if (candidatoTracked.Filiacao != null)
        {
            _dbContext.Filiacoes.Remove(candidatoTracked.Filiacao);
        }
  
        candidatoTracked.Nome = entidade.Nome;
        candidatoTracked.Cpf = entidade.Cpf;
        candidatoTracked.DataNascimento = entidade.DataNascimento;
        candidatoTracked.LinkedInUrl = entidade.LinkedInUrl;
        candidatoTracked.ResumoProfissional = entidade.ResumoProfissional;

        candidatoTracked.Filiacao = entidade.Filiacao;
        candidatoTracked.Endereco = entidade.Endereco;
        candidatoTracked.Telefones = entidade.Telefones;
        candidatoTracked.Cursos = entidade.Cursos;
        candidatoTracked.Formacoes = entidade.Formacoes;
        candidatoTracked.Habilidades = entidade.Habilidades;
        candidatoTracked.Experiencias = entidade.Experiencias;
        
        await _dbContext.SaveChangesAsync();

        return candidatoTracked;
    }


    public async Task<Candidato> ObterPorId(long id)
    {
        return await _dbContext.Candidatos.AsNoTracking()
            .Include(c => c.Telefones)
            .Include(c => c.Cursos)
            .Include(c => c.Formacoes)
            .Include(c => c.Habilidades)
            .Include(c => c.Experiencias)
            .Include(c => c.Endereco)
                .ThenInclude(e => e.Cidade)
                .ThenInclude(c => c.Estado)
            .Include(c => c.Filiacao)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Candidato> ObterPorUsuarioId(long usuarioId)
    {
        return await _dbContext.Candidatos.AsNoTracking()
            .Include(c => c.Telefones)
            .Include(c => c.Cursos)
            .Include(c => c.Formacoes)
            .Include(c => c.Habilidades)
            .Include(c => c.Experiencias)
            .Include(c => c.Endereco)
                .ThenInclude(e => e.Cidade)
                .ThenInclude(c => c.Estado)
            .Include(c => c.Filiacao)
            .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);
    }

    public async Task<List<Candidato>> ObterTodos()
    {
        return await _dbContext.Candidatos.AsNoTracking()
            .Include(c => c.Telefones)
            .Include(c => c.Cursos)
            .Include(c => c.Formacoes)
            .Include(c => c.Habilidades)
            .Include(c => c.Experiencias)
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
            .Include(c => c.Formacoes)
            .Include(c => c.Habilidades)
            .Include(c => c.Experiencias)
            .Include(c => c.Endereco)
                .ThenInclude(e => e.Cidade)
                    .ThenInclude(c => c.Estado)
            .Include(c => c.Filiacao)
            .FirstOrDefaultAsync(x => x.Id == id);
            
        if (candidato == null)
        {
            throw new Exception($"Candidato com o ID: {id} não foi encontrado no banco de dados");
        }
        
        if (candidato.Telefones != null) _dbContext.Telefones.RemoveRange(candidato.Telefones);
        if (candidato.Cursos != null) _dbContext.Cursos.RemoveRange(candidato.Cursos);
        if (candidato.Formacoes != null) _dbContext.Formacoes.RemoveRange(candidato.Formacoes);
        if (candidato.Habilidades != null) _dbContext.Habilidades.RemoveRange(candidato.Habilidades);
        if (candidato.Experiencias != null) _dbContext.Experiencias.RemoveRange(candidato.Experiencias);
        
        if (candidato.Endereco != null)
        {
            _dbContext.Enderecos.Remove(candidato.Endereco);
            if (candidato.Endereco.Cidade != null)
            {
                _dbContext.Cidades.Remove(candidato.Endereco.Cidade);
                if (candidato.Endereco.Cidade.Estado != null)
                {
                    _dbContext.Estados.Remove(candidato.Endereco.Cidade.Estado);
                }
            }
        }

        if (candidato.Filiacao != null) _dbContext.Filiacoes.Remove(candidato.Filiacao);
        
        _dbContext.Candidatos.Remove(candidato);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}