using candidatos.Models;

namespace candidato.DataAccess
{
    public interface IVagaDao
    {
        Task<List<Vaga>> ObterAbertas();
        Task<List<Vaga>> ObterPorRecrutador(long recrutadorId);
        Task<Vaga?> ObterPorId(long id);
        Task<Vaga> Adicionar(Vaga vaga);
    }
}
