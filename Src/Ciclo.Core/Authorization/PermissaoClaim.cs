using Ciclo.Core.Extensions;

namespace Ciclo.Core.Authorization;

public class PermissaoClaim
{
    public string Nome { get; set; } = null!;
    public string Tipo { get; set; } = null!;

    public const string Separador = ":";
    
    public override string ToString() => $"{Nome}{Separador}{Tipo}";

    public static string FormatarParaClaim(string nome, EPermissaoTipo tipo)
        => $"{nome}{Separador}{tipo.ToDescriptionString()}";
    
    public static bool Verificar(string claim, string nome, string tipo)
    {
        var parts = claim.Split(Separador);
        var claimNome = (string)(parts.GetValue(0) ?? string.Empty);
        var claimTipo = (string)(parts.GetValue(1) ?? string.Empty);

        return claimNome == nome && claimTipo.Contains(tipo);
    }
}