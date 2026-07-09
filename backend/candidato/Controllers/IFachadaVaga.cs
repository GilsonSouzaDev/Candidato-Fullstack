using candidatos.Models;

namespace candidato.Controllers
{
    public interface IFachadaVaga
    {
        Task<List<object>> ObterAbertas();
        Task<List<Vaga>> ObterMinhasVagas(long userId);
        Task<object?> ObterDetalhes(long id);
        Task<Vaga> CriarVaga(Vaga dto, long userId);
    }
}
