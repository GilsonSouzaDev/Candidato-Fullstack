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
            dto.CriadoPorId = userId;
            dto.StatusAberta = true;
            dto.DataCriacao = DateTime.UtcNow;

            return await _vagaDao.Adicionar(dto);
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
