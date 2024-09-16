using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ciclo.Application.BackgroundJob.Job;

public abstract class BaseJob : BackgroundService
{
    private const string DataFormat = "dd-MM-yyyy HH:mm:ss:fff";
    private bool _firstExecution = true;
    private readonly EWorkerNames _instance;
    private readonly IWorkerParams _workerParams;
    private readonly ILogger<BaseJob> _logger;

    protected BaseJob(IWorkerParams workerParams, ILogger<BaseJob> logger, EWorkerNames workerName)
    {
        _workerParams = workerParams;
        _logger = logger;
        _instance = workerName;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("{A0} - {A1} | {A2} | {A3}", DateTime.Now.ToString(DataFormat),
            "START", _instance, nameof(ExecuteAsync));
        
        stoppingToken.Register(ActionStop);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Delay(stoppingToken);
            if (!_workerParams.AreWorkersEnabled())
            {
                _logger.LogWarning("{Date} - {Instance} | The start of this worker was aborted because worker running is disabled.", 
                    DateTime.Now.ToString(DataFormat),
                    _instance);
                continue;
            }
            
            await ActionRunAsync(stoppingToken);
        }
        
        ActionStop();
    }

    public virtual async Task ActionRunAsync(CancellationToken cancellationToken = default)
    {
        if (_workerParams.IsWorkerRunning(_instance))
        {
            _logger.LogWarning("{Date} - {Instance} | The start of this worker was aborted because already exists an instance running since {Running}.", 
                DateTime.Now.ToString(DataFormat),
                _instance,
                _workerParams.GetRunningWorkerStartTime(_instance));
            return;
        }
        
        _logger.LogInformation("{Date} - {InstanceFirst} | Setting the worker {InstanceSecond} to running state.",
            DateTime.Now.ToString(DataFormat),
            _instance, 
            _instance);
        
        _workerParams.SetWorkerRunningState(_instance);

        try
        {
            await RunAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "{Date} - {Instance} was throw an exception.", DateTime.Now.ToString(DataFormat),
                _instance);
        }
        
        _logger.LogInformation("{Date} - {InstanceFirst} | Setting the worker {InstanceSecond} to stopped state.",
            DateTime.Now.ToString(DataFormat),
            _instance, 
            _instance);
        
        _workerParams.UnsetWorkerRunningState(_instance);
        _workerParams.SetLastRun(_instance);
    }

    protected abstract Task<bool> RunAsync(CancellationToken cancellationToken = default);

    private async Task Delay(CancellationToken cancellationToken)
    {
        if (!_firstExecution)
        {
            await Task.Delay(DelayTime(), cancellationToken);
        }
            
        _firstExecution = false;
    }
    
    protected virtual int DelayTime() => (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
    
    protected void ActionStop()
    {
        _logger.LogInformation("{A0} - {A1} | {A2} | {A3}", DateTime.Now.ToString(DataFormat),
            "STOP", _instance, nameof(ActionStop));
    }
}