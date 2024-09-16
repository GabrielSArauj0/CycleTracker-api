using System.ComponentModel;

namespace Ciclo.Core.Authorization;

public enum EPermissaoTipo
{
    [Description("R")]
    Read = 1,

    [Description("W")]
    Write = 2,

    [Description("D")]
    Delete = 3,

    [Description("RW")]
    ReadWrite = 4,

    [Description("WD")]
    WriteDelete = 5,

    [Description("RD")]
    ReadDelete = 6,

    [Description("RWD")]
    Full = 7
}

public enum EPermissaoNivel
{
    R = 1,
    W = 2,
    D = 3
}