using candidato.Exceptions;
using candidato.DataAccess;
using candidatos.Models;
using FluentValidation;

namespace candidato.Controllers.Fachadas
{
    public class FachadaCandidato : IFachadaCandidato
    {
        private readonly IDao _candidatoDao;
        private readonly IValidator<Candidato> _validator;

        public FachadaCandidato(IDao candidatoDao, IValidator<Candidato> validator)
        {
            _candidatoDao = candidatoDao;
            _validator = validator;
        }

        public async Task<Candidato> Adicionar(Candidato entidade)
        {
            var result = await _validator.ValidateAsync(entidade);
            if (!result.IsValid)
            {
                var errorMessages = string.Join("\n", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidacaoException(errorMessages);
            }

            return await _candidatoDao.Adicionar(entidade);
        }

        public async Task<Candidato> Atualizar(long id, Candidato entidade)
        {
            var result = await _validator.ValidateAsync(entidade);
            if (!result.IsValid)
            {
                var errorMessages = string.Join("\n", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidacaoException(errorMessages);
            }

            return await _candidatoDao.Atualizar(entidade);
        }

        public async Task<Candidato> ObterPorId(long id)
        {
            return await _candidatoDao.ObterPorId(id);
        }

        public  async Task<List<Candidato>> ObterTodos()
        {
            return await _candidatoDao.ObterTodos();
        }

        public async Task<bool> Remover(long id)
        {
            return await _candidatoDao.Remover(id);
        }
    }
}
