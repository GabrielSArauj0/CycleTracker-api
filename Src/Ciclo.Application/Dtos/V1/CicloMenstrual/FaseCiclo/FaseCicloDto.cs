namespace Ciclo.Application.Dtos.V1.CicloMenstrual.FaseCiclo;

public class FaseCicloDto
{
    public string Nome { get; set; } = null!;
    public DateTime Inicio { get; set; }
    public DateTime Fim { get; set; }
}