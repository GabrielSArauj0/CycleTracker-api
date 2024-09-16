namespace Ciclo.Core.Settings;

public class JwtSettings
{
    public int ExpiracaoHoras { get; set; }
    public string Emissor { get; set; } = string.Empty;
    public string ValidoEm { get; set; } = string.Empty;
    public string CaminhoKeys { get; set; } = string.Empty;
    
    public List<string> Audiences()
    {
        var audiences = ValidoEm.Split(',').ToList();
        audiences.Add(Emissor);
        return audiences;
    }
}