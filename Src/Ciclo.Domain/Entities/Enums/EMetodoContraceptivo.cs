using System.ComponentModel;

namespace Ciclo.Domain.Entities.Enums;

public enum EMetodoContraceptivo
{
    [Description("Pílula Contraceptiva")]
    Pilula = 1,

    [Description("Dispositivo Intrauterino (DIU)")]
    Diu = 2,

    [Description("Injeção Contraceptiva")]
    Injecao = 3,

    [Description("Adesivo Contraceptivo")]
    Adesivo = 4,

    [Description("Implante Contraceptivo")]
    Implante = 5,

    [Description("Anel Vaginal")]
    AnelVaginal = 6,

    [Description("Contraceptivo de Emergência")]
    ContraceptivoEmergencia = 7,

    [Description("Laqueadura")]
    Laqueadura = 8,

    [Description("Nenhum")]
    Nenhum = 9
}