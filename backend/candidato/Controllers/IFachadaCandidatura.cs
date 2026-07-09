using candidato.DTOs;

namespace candidato.Controllers
{
    public interface IFachadaCandidatura
    {
        Task Aplicar(CandidaturaDto dto);
        Task<List<object>> ObterCandidaturasDaVaga(long vagaId, long recrutadorId);
        Task AtualizarStatus(long candidaturaId, string novoStatus, long recrutadorId);
    }
}
