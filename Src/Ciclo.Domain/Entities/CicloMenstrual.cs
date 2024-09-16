using Ciclo.Domain.Contracts;
using Ciclo.Domain.Entities.Enums;
using Ciclo.Domain.Validators;
using FluentValidation.Results;

namespace Ciclo.Domain.Entities;

public class CicloMenstrual : Entity, IAggregateRoot, ISoftDelete
{
    public int UsuarioId { get; set; }
    public DateTime DataInicioUltimaMenstruacao { get; set; } 
    public int DuracaoCiclo { get; set; }
    public int DuracaoMenstruacao { get; set; }
    public EMetodoContraceptivo? MetodoContraceptivo { get; set; }
    public EIntensidadeFluxo IntensidadeFluxo { get; set; }
    public bool Ativo { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
    
    public override bool Validate(out ValidationResult validationResult)
    {
        validationResult = new CicloMenstrualValidator().Validate(this);
        return validationResult.IsValid;
    }
}