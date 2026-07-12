using candidato.DataAccess;
using candidatos.Models;

namespace candidato.Controllers.Fachadas
{
    public class FachadaVaga : IFachadaVaga
    {
        private readonly IVagaDao _vagaDao;

        public FachadaVaga(IVagaDao vagaDao)
        {
            _vagaDao = vagaDao;
        }

        public async Task<List<Vaga>> ObterAbertas()
        {
            return await _vagaDao.ObterAbertas();
        }

        public async Task<Vaga?> ObterDetalhes(long id)
        {
            return await _vagaDao.ObterPorId(id);
        }

        public async Task<List<Vaga>> ObterMinhasVagas(long userId)
        {
            return await _vagaDao.ObterPorRecrutador(userId);
        }

        public async Task<Vaga> CriarVaga(Vaga dto, long userId)
        {
            var vaga = new Vaga
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Requisitos = dto.Requisitos,
                Salario = dto.Salario,
                StatusAberta = true,
                CriadoPorId = userId
            };

            return await _vagaDao.Adicionar(vaga);
        }

        public async Task<bool> RemoverVaga(long id, long userId)
        {
            var vaga = await _vagaDao.ObterPorId(id);
            if (vaga == null || vaga.CriadoPorId != userId)
                return false;

            return await _vagaDao.Remover(id);
        }
    }
}
