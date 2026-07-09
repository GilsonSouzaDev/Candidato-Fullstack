using candidatos.Models;

namespace candidato.DataAccess
{
    public interface IUsuarioDao
    {
        Task<Usuario?> ObterPorEmail(string email);
        Task<Usuario> Adicionar(Usuario usuario);
        Task<Usuario?> ObterPorId(long id);
    }
}
