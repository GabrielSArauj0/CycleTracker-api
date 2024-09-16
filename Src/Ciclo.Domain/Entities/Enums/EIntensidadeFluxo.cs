using System.ComponentModel;

namespace Ciclo.Domain.Entities.Enums;

public enum EIntensidadeFluxo
{
    [Description("Fluxo Leve")]
    Leve = 1,

    [Description("Fluxo Moderado")]
    Moderado = 2,

    [Description("Fluxo Intenso")]
    Intenso = 3,

    [Description("Sangramento Intermenstrual")]
    Intermenstrual = 4,

    [Description("Ausência de Fluxo")]
    Ausencia = 5
}