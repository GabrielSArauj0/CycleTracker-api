using Ciclo.Domain.Contracts;
using FluentValidation.Results;

namespace Ciclo.Domain.Entities;

public abstract class Entity : BaseEntity, ITracking
{
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }
    public int CriadoPor { get; set; }
    public int AtualizadoPor { get; set; }

    public virtual bool Validate(out ValidationResult validationResult)
    {
        validationResult = new ValidationResult();
        return validationResult.IsValid;
    }
}