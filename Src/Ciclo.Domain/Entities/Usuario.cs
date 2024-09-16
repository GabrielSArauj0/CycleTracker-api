using Ciclo.Domain.Contracts;
using Ciclo.Domain.Validators;
using FluentValidation.Results;

namespace Ciclo.Domain.Entities;

public class Usuario : Entity, IAggregateRoot, ISoftDelete
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public bool Ativo { get; set; }

    public virtual List<CicloMenstrual> CiclosMenstruais { get; set; } = new();
    
    public override bool Validate(out ValidationResult validationResult)
    {
        validationResult = new UsuarioValidator().Validate(this);
        return validationResult.IsValid;
    }
}