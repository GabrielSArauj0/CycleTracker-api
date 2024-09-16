using Ciclo.Domain.Entities;
using FluentValidation;

namespace Ciclo.Domain.Validators;

public class CicloMenstrualValidator : AbstractValidator<CicloMenstrual>
{
    public CicloMenstrualValidator()
    {
        RuleFor(a => a.UsuarioId)
            .NotEmpty()
            .NotNull();
        
        RuleFor(a => a.DataInicioUltimaMenstruacao)
            .NotEmpty()
            .NotNull();

        RuleFor(a => a.DuracaoCiclo)
            .NotEmpty()
            .NotNull();

        RuleFor(a => a.DuracaoMenstruacao)
            .NotEmpty()
            .NotNull();
        
        
    }
}