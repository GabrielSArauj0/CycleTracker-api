namespace Ciclo.Application.Dtos.V1.Anotacoes;

public class AnotacaoDeletarDto
{
    public int Id { get; set; }
    
    public DateOnly Date { get; set; }
    
    public string Titulo { get; set; }
    
    public string Descricao { get; set; }

    public bool Ativo { get; set; }
}