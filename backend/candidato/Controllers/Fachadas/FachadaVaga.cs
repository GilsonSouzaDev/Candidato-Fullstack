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

        public async Task<List<object>> ObterAbertas()
        {
            var vagas = await _vagaDao.ObterAbertas();
            return vagas.Select(v => (object)new {
                v.Id,
                v.Titulo,
                v.Descricao,
                v.Requisitos,
                v.Salario,
                v.DataCriacao,
                Recrutador = v.CriadoPor != null ? v.CriadoPor.Nome : "Desconhecido"
            }).ToList();
        }

        public async Task<object?> ObterDetalhes(long id)
        {
            var vaga = await _vagaDao.ObterPorId(id);
            if (vaga == null) return null;

            return new {
                vaga.Id,
                vaga.Titulo,
                vaga.Descricao,
                vaga.Requisitos,
                vaga.Salario,
                vaga.StatusAberta,
                Recrutador = vaga.CriadoPor?.Nome
            };
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
    }
}
