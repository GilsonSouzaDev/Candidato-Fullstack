using candidatos.Data;
using candidatos.Models;
using Microsoft.EntityFrameworkCore;

namespace candidato.DataAccess.daos
{
    public class UsuarioDao : IUsuarioDao
    {
        private readonly CandidatoContext _context;

        public UsuarioDao(CandidatoContext context)
        {
            _context = context;
        }

        public async Task<Usuario> Adicionar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario?> ObterPorEmail(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario?> ObterPorId(long id)
        {
            return await _context.Usuarios.FindAsync(id);
        }
    }
}
