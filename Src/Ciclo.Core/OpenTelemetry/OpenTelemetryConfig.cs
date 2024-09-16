namespace Ciclo.Core.OpenTelemetry;

public class OpenTelemetryConfig
{
    public bool Ativo { get; set; }
    public string ServiceName { get; set; }
    public string OtlpEndpoint { get; set; }
}