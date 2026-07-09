using candidato.DataAccess;
using candidatos.Models;
using candidato.DTOs;

namespace candidato.Controllers.Fachadas
{
    public class FachadaCandidatura : IFachadaCandidatura
    {
        private readonly ICandidaturaDao _candidaturaDao;
        private readonly IVagaDao _vagaDao;
        private readonly IDao _candidatoDao; // Usando o DAO original de Candidato

        public FachadaCandidatura(ICandidaturaDao candidaturaDao, IVagaDao vagaDao, IDao candidatoDao)
        {
            _candidaturaDao = candidaturaDao;
            _vagaDao = vagaDao;
            _candidatoDao = candidatoDao;
        }

        public async Task Aplicar(CandidaturaDto dto)
        {
            var vaga = await _vagaDao.ObterPorId(dto.VagaId);
            if (vaga == null || !vaga.StatusAberta)
                throw new Exception("Vaga não encontrada ou fechada.");

            var candidato = await _candidatoDao.ObterPorId(dto.CandidatoId);
            if (candidato == null)
                throw new Exception("Candidato não encontrado. Registre-se primeiro.");

            var exists = await _candidaturaDao.VerificarExistencia(dto.VagaId, dto.CandidatoId);
            if (exists)
                throw new Exception("Você já se candidatou para esta vaga.");

            var candidatura = new Candidatura
            {
                VagaId = dto.VagaId,
                CandidatoId = dto.CandidatoId,
                StatusCandidatura = "Triagem"
            };

            await _candidaturaDao.Adicionar(candidatura);
        }

        public async Task<List<object>> ObterCandidaturasDaVaga(long vagaId, long recrutadorId)
        {
            var vaga = await _vagaDao.ObterPorId(vagaId);
            if (vaga == null) throw new Exception("NOT_FOUND");
            if (vaga.CriadoPorId != recrutadorId) throw new Exception("FORBIDDEN");

            var candidaturas = await _candidaturaDao.ObterPorVaga(vagaId);
            
            return candidaturas.Select(c => (object)new {
                c.Id,
                c.StatusCandidatura,
                c.DataAplicacao,
                CandidatoNome = c.Candidato?.Nome ?? "Desconhecido",
                CandidatoEmail = "nao-informado@teste.com"
            }).ToList();
        }

        public async Task AtualizarStatus(long candidaturaId, string novoStatus, long recrutadorId)
        {
            var candidatura = await _candidaturaDao.ObterPorId(candidaturaId);
            if (candidatura == null) throw new Exception("NOT_FOUND");
            if (candidatura.Vaga == null || candidatura.Vaga.CriadoPorId != recrutadorId) throw new Exception("FORBIDDEN");

            candidatura.StatusCandidatura = novoStatus;
            await _candidaturaDao.Atualizar(candidatura);
        }
    }
}
