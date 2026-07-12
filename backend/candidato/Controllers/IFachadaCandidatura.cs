using candidato.DTOs;

namespace candidato.Controllers
{
    public interface IFachadaCandidatura
    {
        Task Aplicar(CandidaturaDto dto, long userId);
        Task<List<object>> ObterCandidaturasDaVaga(long vagaId, long recrutadorId);
        Task<List<long>> ObterMinhasInscricoes(long userId);
        Task AtualizarStatus(long candidaturaId, string novoStatus, long recrutadorId);
        Task Cancelar(long vagaId, long userId);
    }
}
