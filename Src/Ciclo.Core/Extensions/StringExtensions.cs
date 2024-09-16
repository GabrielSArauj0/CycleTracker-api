namespace Ciclo.Core.Extensions;

public static class StringExtensions
{
    public static string? SomenteNumeros(this string? value)
    {
        return string.IsNullOrWhiteSpace(value) 
            ? null 
            : string.Join("", System.Text.RegularExpressions.Regex.Split(value, @"[^\d]"));
    }
}