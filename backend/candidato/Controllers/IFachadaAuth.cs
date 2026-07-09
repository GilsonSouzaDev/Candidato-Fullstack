using candidato.Controllers;
using candidatos.Models;

namespace candidato.Controllers
{
    public interface IFachadaAuth
    {
        Task<Usuario> Registrar(RegisterDto dto);
        Task<string> Login(LoginDto dto);
    }
}
