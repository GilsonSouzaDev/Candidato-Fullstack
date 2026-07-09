using candidatos.Data;
using candidatos.Models;
using Microsoft.EntityFrameworkCore;

namespace candidato.DataAccess.daos
{
    public class CandidaturaDao : ICandidaturaDao
    {
        private readonly CandidatoContext _context;

        public CandidaturaDao(CandidatoContext context)
        {
            _context = context;
        }

        public async Task<bool> VerificarExistencia(long vagaId, long candidatoId)
        {
            return await _context.Candidaturas
                .AnyAsync(c => c.VagaId == vagaId && c.CandidatoId == candidatoId);
        }

        public async Task<Candidatura> Adicionar(Candidatura candidatura)
        {
            _context.Candidaturas.Add(candidatura);
            await _context.SaveChangesAsync();
            return candidatura;
        }

        public async Task<List<Candidatura>> ObterPorVaga(long vagaId)
        {
            return await _context.Candidaturas
                .Include(c => c.Candidato)
                .Where(c => c.VagaId == vagaId)
                .OrderByDescending(c => c.DataAplicacao)
                .ToListAsync();
        }

        public async Task<Candidatura?> ObterPorId(long id)
        {
            return await _context.Candidaturas
                .Include(c => c.Vaga)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Candidatura> Atualizar(Candidatura candidatura)
        {
            _context.Candidaturas.Update(candidatura);
            await _context.SaveChangesAsync();
            return candidatura;
        }
    }
}
