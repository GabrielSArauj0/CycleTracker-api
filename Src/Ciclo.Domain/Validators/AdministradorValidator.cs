using Ciclo.Domain.Entities;
using FluentValidation;

namespace Ciclo.Domain.Validators;

public class AdministradorValidator : AbstractValidator<Administrador>
{
    public AdministradorValidator()
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
            .MinimumLength(8)
            .WithMessage("A senha deve ter no mínimo 8 caracteres");
    }
}