using Microsoft.Extensions.Logging;

namespace Ciclo.Application.BackgroundJob.Job;

public class AtualizarAvisosJob : BaseJob
{
    private readonly IServiceProvider _serviceProvider;

    public AtualizarAvisosJob(IWorkerParams workerParams, IServiceProvider serviceProvider,
        ILogger<AtualizarAvisosJob> logger) : base(workerParams, logger, 
        EWorkerNames.AtualizarAvisos)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task<bool> RunAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    protected override int DelayTime() => (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
}