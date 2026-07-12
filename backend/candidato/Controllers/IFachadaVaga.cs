using candidatos.Models;

namespace candidato.Controllers
{
    public interface IFachadaVaga
    {
        Task<List<Vaga>> ObterAbertas();
        Task<List<Vaga>> ObterMinhasVagas(long userId);
        Task<Vaga?> ObterDetalhes(long id);
        Task<Vaga> CriarVaga(Vaga dto, long userId);
        Task<bool> RemoverVaga(long id, long userId);
    }
}
