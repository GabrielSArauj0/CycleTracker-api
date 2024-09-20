namespace Ciclo.Application.Dtos.V1.Anotacoes;

public class AtualizarAnotacaoDto
{
    public DateOnly Date { get; set; }
    public int Id { get; set; }
    
    public string Titulo { get; set; }
    
    public string Descricao { get; set; }
    
}