using System.ComponentModel.DataAnnotations;

namespace Ciclo.Application.Dtos.V1.Anotacoes;

public class AnotacaoDto
{
    
    public DateOnly Date { get; set; } 
    
    public int Id { get; set; }
    
    public string Titulo { get; set; } = null!;
    
    [Required(ErrorMessage = "A descrição é obrigatória")]
    public string Descricao { get; set; } = null!;
}