using candidatos.Models;

namespace candidato.DataAccess;

public interface IDao
{
    Task<List<Candidato>> ObterTodos();
    Task<Candidato> Adicionar(Candidato entidade);
    Task<Candidato> ObterPorId(long id);
    Task<Candidato> Atualizar(Candidato entidade);
    Task<bool> Remover(long id);

}
