using candidatos.Models;

namespace candidato.Controllers
{
    public interface IFachadaCandidato
    {
        Task<List<Candidato>> ObterTodos();
        Task<Candidato> Adicionar(Candidato entidade);
        Task<Candidato> ObterPorId(long id);
        Task<Candidato> Atualizar(long id,Candidato entidade);
        Task<bool> Remover(long id);
    }
}
