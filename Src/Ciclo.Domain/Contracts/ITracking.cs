namespace Ciclo.Domain.Contracts;

public interface ITracking
{
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }
    public int CriadoPor { get; set; }
    public int AtualizadoPor { get; set; }
}