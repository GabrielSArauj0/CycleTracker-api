namespace Ciclo.Application.Dtos.V1.Anotacoes;

public class AtualizarAnotacaoDto
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    
    public virtual List<Domain.Entities.Anotacoes> AnotacoesList { get; set; } = new();
}