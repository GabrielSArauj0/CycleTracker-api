using Ciclo.Domain.Entities;
using FluentValidation;

namespace Ciclo.Domain.Validators;

public class UsuarioValidator : AbstractValidator<Usuario>
{
    public UsuarioValidator()
    {
        RuleFor(a => a.Nome)
            .NotEmpty()
            .WithMessage("O nome não pode ser vazio")
            .Length(3, 120)
            .WithMessage("O nome deve ter no mínimo 3 e no máximo 120 caracteres");

        RuleFor(a => a.Email)
            .EmailAddress();
        
        RuleFor(a => a.Senha)
            .NotEmpty()
            .WithMessage("A senha não pode ser vazia")
            .MinimumLength(3)
            .WithMessage("A senha deve ter no mínimo 3 caracteres");
    }
}