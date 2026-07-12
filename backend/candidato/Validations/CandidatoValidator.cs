using FluentValidation;
using candidatos.Models;

namespace candidato.Validations;

public class CandidatoValidator : AbstractValidator<Candidato>
{
    public CandidatoValidator()
    {
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("O campo candidato deve ser preenchido.")
            .MinimumLength(5).WithMessage("O campo candidato deve ter no mínimo 5 caracteres.")
            .MaximumLength(150).WithMessage("O campo candidato deve ter no máximo 150 caracteres.");

        RuleFor(c => c.Cpf).NotEmpty().WithMessage("O CPF é obrigatório.");
        RuleFor(c => c.DataNascimento).NotNull().WithMessage("A Data de Nascimento é obrigatória.");

        RuleFor(c => c.Filiacao).NotNull().WithMessage("A filiação é obrigatória.");
        
        When(c => c.Filiacao != null, () =>
        {
            RuleFor(c => c.Filiacao.NomeMae)
                .NotEmpty().WithMessage("O campo Mãe deve ser preenchido.")
                .MinimumLength(5).WithMessage("O campo Mãe deve ter no mínimo 5 caracteres.")
                .MaximumLength(150).WithMessage("O campo Mãe deve ter no máximo 150 caracteres.");
        });

        RuleFor(c => c.Endereco).NotNull().WithMessage("O endereço é obrigatório.");

        When(c => c.Endereco != null, () =>
        {
            RuleFor(c => c.Endereco.Logradouro).NotEmpty().WithMessage("O campo Logradouro deve ser preenchido.");
            RuleFor(c => c.Endereco.Numero).NotEmpty().WithMessage("O campo Numero deve ser preenchido.");
            RuleFor(c => c.Endereco.Cep).NotEmpty().WithMessage("O campo Cep deve ser preenchido.");
            
            RuleFor(c => c.Endereco.Cidade).NotNull().WithMessage("A cidade é obrigatória.");
            When(c => c.Endereco.Cidade != null, () =>
            {
                RuleFor(c => c.Endereco.Cidade.Nome).NotEmpty().WithMessage("O campo Cidade deve ser preenchido.");
                RuleFor(c => c.Endereco.Cidade.Estado).NotNull().WithMessage("O estado é obrigatório.");
                
                When(c => c.Endereco.Cidade.Estado != null, () =>
                {
                    RuleFor(c => c.Endereco.Cidade.Estado.Nome).NotEmpty().WithMessage("O campo Estado deve ser preenchido.");
                    RuleFor(c => c.Endereco.Cidade.Estado.Sigla).NotEmpty().WithMessage("O campo Sigla deve ser preenchido.");
                });
            });
        });

        RuleFor(c => c.Telefones)
            .NotEmpty().WithMessage("É necessário adicionar pelo menos um telefone.")
            .Must(t => t == null || t.Count <= 3).WithMessage("Não é permitido adicionar mais de 3 telefones ao candidato.");

        RuleFor(c => c.Cursos)
            .NotEmpty().WithMessage("É necessário adicionar pelo menos um curso.")
            .Must(c => c == null || c.Count <= 3).WithMessage("Não é permitido adicionar mais de 3 cursos ao candidato.");
    }
}
