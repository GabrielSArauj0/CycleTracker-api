namespace Ciclo.Application.Dtos.V1.Anotacoes;

public class AdicionarAnotacaoDto
{
    public int UserId { get; set; }
    
    public DateOnly Date { get; set; }

    public string Titulo { get; set; } = null!;

    public string AnotacaoTexto { get; set; } = null!;
}