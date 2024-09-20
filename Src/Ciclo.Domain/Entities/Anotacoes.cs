using Ciclo.Domain.Contracts;

namespace Ciclo.Domain.Entities;

public class Anotacoes : Entity, IAggregateRoot, ISoftDelete
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public DateOnly Date { get; set; }
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;

    public bool Ativo { get; set; }
    
    public virtual Usuario Usuario { get; set; } = null!;
}