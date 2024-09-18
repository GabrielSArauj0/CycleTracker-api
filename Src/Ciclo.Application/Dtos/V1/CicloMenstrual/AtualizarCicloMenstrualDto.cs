using Ciclo.Domain.Entities.Enums;

namespace Ciclo.Application.Dtos.V1.CicloMenstrual;

public class AtualizarCicloMenstrualDto
{
    public int Id { get; set; }
    public DateTime DataInicioUltimaMenstruacao { get; set; } 
    public int DuracaoCiclo { get; set; }
    public int DuracaoMenstruacao { get; set; }
    public EMetodoContraceptivo? MetodoContraceptivo { get; set; }
    public EIntensidadeFluxo IntensidadeFluxo { get; set; }
    public bool Ativo { get; set; }
}