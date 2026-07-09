using candidatos.Data;
using candidatos.Models;
using Microsoft.EntityFrameworkCore;

namespace candidato.DataAccess.daos
{
    public class VagaDao : IVagaDao
    {
        private readonly CandidatoContext _context;

        public VagaDao(CandidatoContext context)
        {
            _context = context;
        }

        public async Task<List<Vaga>> ObterAbertas()
        {
            return await _context.Vagas
                .Include(v => v.CriadoPor)
                .Where(v => v.StatusAberta)
                .OrderByDescending(v => v.DataCriacao)
                .ToListAsync();
        }

        public async Task<List<Vaga>> ObterPorRecrutador(long recrutadorId)
        {
            return await _context.Vagas
                .Where(v => v.CriadoPorId == recrutadorId)
                .OrderByDescending(v => v.DataCriacao)
                .ToListAsync();
        }

        public async Task<Vaga?> ObterPorId(long id)
        {
            return await _context.Vagas
                .Include(v => v.CriadoPor)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vaga> Adicionar(Vaga vaga)
        {
            _context.Vagas.Add(vaga);
            await _context.SaveChangesAsync();
            return vaga;
        }
    }
}
