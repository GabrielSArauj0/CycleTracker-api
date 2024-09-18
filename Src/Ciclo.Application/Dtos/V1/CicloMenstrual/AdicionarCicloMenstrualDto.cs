using Ciclo.Domain.Entities.Enums;

namespace Ciclo.Application.Dtos.V1.CicloMenstrual;

public class AdicionarCicloMenstrualDto
{
    public int UsuarioId { get; set; }
    public DateTime DataInicioUltimaMenstruacao { get; set; } 
    public int DuracaoCiclo { get; set; }
    public int DuracaoMenstruacao { get; set; }
    public EMetodoContraceptivo? MetodoContraceptivo { get; set; }
    public EIntensidadeFluxo IntensidadeFluxo { get; set; }
    public bool Ativo { get; set; }
}