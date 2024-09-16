using Ciclo.Domain.Contracts;

namespace Ciclo.Domain.Entities;

public abstract class BaseEntity : IEntity
{
    public int Id { get; set; }
}