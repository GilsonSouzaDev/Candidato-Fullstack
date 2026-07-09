using candidatos.Models;

namespace candidato.DataAccess
{
    public interface ICandidaturaDao
    {
        Task<bool> VerificarExistencia(long vagaId, long candidatoId);
        Task<Candidatura> Adicionar(Candidatura candidatura);
        Task<List<Candidatura>> ObterPorVaga(long vagaId);
        Task<Candidatura?> ObterPorId(long id);
        Task<Candidatura> Atualizar(Candidatura candidatura);
    }
}
