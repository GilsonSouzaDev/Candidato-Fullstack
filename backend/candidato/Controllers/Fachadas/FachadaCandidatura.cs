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

        public async Task Aplicar(CandidaturaDto dto, long userId)
        {
            var vaga = await _vagaDao.ObterPorId(dto.VagaId);
            if (vaga == null || !vaga.StatusAberta)
                throw new Exception("Vaga não encontrada ou fechada.");

            var candidato = await _candidatoDao.ObterPorUsuarioId(userId);
            if (candidato == null)
                throw new Exception("Candidato não encontrado. Registre seu currículo primeiro.");

            var exists = await _candidaturaDao.VerificarExistencia(dto.VagaId, candidato.Id);
            if (exists)
                throw new Exception("Você já se candidatou para esta vaga.");

            var candidatura = new Candidatura
            {
                VagaId = dto.VagaId,
                CandidatoId = candidato.Id,
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
                CandidatoId = c.CandidatoId,
                CandidatoNome = c.Candidato?.Nome ?? "Desconhecido",
                CandidatoEmail = c.Candidato?.Usuario?.Email ?? "nao-informado@teste.com"
            }).ToList();
        }

        public async Task<List<long>> ObterMinhasInscricoes(long userId)
        {
            var candidato = await _candidatoDao.ObterPorUsuarioId(userId);
            if (candidato == null) return new List<long>();

            var candidaturas = await _candidaturaDao.ObterPorCandidato(candidato.Id);
            return candidaturas.Select(c => c.VagaId).ToList();
        }

        public async Task AtualizarStatus(long candidaturaId, string novoStatus, long recrutadorId)
        {
            var candidatura = await _candidaturaDao.ObterPorId(candidaturaId);
            if (candidatura == null) throw new Exception("NOT_FOUND");
            if (candidatura.Vaga == null || candidatura.Vaga.CriadoPorId != recrutadorId) throw new Exception("FORBIDDEN");

            candidatura.StatusCandidatura = novoStatus;
            await _candidaturaDao.Atualizar(candidatura);
        }

        public async Task Cancelar(long vagaId, long userId)
        {
            var candidato = await _candidatoDao.ObterPorUsuarioId(userId);
            if (candidato == null) throw new Exception("Candidato não encontrado.");

            var exists = await _candidaturaDao.VerificarExistencia(vagaId, candidato.Id);
            if (!exists) throw new Exception("Inscrição não encontrada.");

            await _candidaturaDao.Deletar(vagaId, candidato.Id);
        }
    }
}
