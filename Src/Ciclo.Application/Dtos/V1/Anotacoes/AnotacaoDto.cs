using Google.Protobuf.WellKnownTypes;

namespace Ciclo.Application.Dtos.V1.Anotacoes;

public class AnotacaoDto
{
    public int Id { get; set; }
    
    public string Titulo { get; set; } = null!;
    
    public string Descricao { get; set; } = null!;
    
}