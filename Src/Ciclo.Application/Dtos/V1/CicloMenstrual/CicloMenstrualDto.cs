using Ciclo.Application.Dtos.V1.Usuario;
using Ciclo.Domain.Entities.Enums;

namespace Ciclo.Application.Dtos.V1.CicloMenstrual;

public class CicloMenstrualDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public DateTime DataInicioUltimaMenstruacao { get; set; } 
    public int DuracaoCiclo { get; set; }
    public int DuracaoMenstruacao { get; set; }
    public EMetodoContraceptivo? MetodoContraceptivo { get; set; }
    public EIntensidadeFluxo IntensidadeFluxo { get; set; }
    public bool Ativo { get; set; }

    public UsuarioDto UsuarioDto { get; set; } = null!;
}