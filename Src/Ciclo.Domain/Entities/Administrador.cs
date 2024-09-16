using Ciclo.Domain.Contracts;
using Ciclo.Domain.Validators;
using FluentValidation.Results;

namespace Ciclo.Domain.Entities;

public class Administrador : Entity, IAggregateRoot, ISoftDelete
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public bool Ativo { get; set; }
    public bool Adm { get; set; }
    
    public override bool Validate(out ValidationResult validationResult)
    {
        validationResult = new AdministradorValidator().Validate(this);
        return validationResult.IsValid;
    }
}