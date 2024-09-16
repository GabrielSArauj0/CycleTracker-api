using System.Globalization;
using Newtonsoft.Json;

namespace Ciclo.Application.BackgroundJob;

public interface IWorkerParams
{
    int WorkerSleepTime { get; set; }

    bool AreWorkersEnabled();
    bool SetWorkersEnabled(bool value);
    bool IsWorkerRunning(EWorkerNames eWorkerName);
    void SetWorkerRunningState(EWorkerNames eWorker);
    void UnsetWorkerRunningState(EWorkerNames eWorker);
    string GetRunningWorkerStartTime(EWorkerNames eWorker);
    DateTime? GetLastRun(EWorkerNames eWorker);
    void SetLastRun(EWorkerNames eWorker, DateTime? date = null);
    object GetStatus();
}

public class WorkerParams : IWorkerParams
{
    private bool _workersEnabled = true;
    private readonly Dictionary<EWorkerNames, DateTime> _runningWorkers = new();
    private readonly Dictionary<EWorkerNames, DateTime> _lastRun = new();

    public int WorkerSleepTime { get; set; } = 6000000;

    public bool AreWorkersEnabled() => _workersEnabled;

    public bool SetWorkersEnabled(bool value)
    {
        _workersEnabled = value;
        return _workersEnabled;
    }

    public bool IsWorkerRunning(EWorkerNames eWorkerName) => _runningWorkers.ContainsKey(eWorkerName);

    public void SetWorkerRunningState(EWorkerNames eWorker)
    {
        if (!IsWorkerRunning(eWorker))
            _runningWorkers.Add(eWorker, DateTime.Now);
    }

    public void UnsetWorkerRunningState(EWorkerNames eWorker)
    {
        if (IsWorkerRunning(eWorker))
            _runningWorkers.Remove(eWorker);
    }

    public string GetRunningWorkerStartTime(EWorkerNames eWorker) 
        => !IsWorkerRunning(eWorker) ? "NOT-RUNNING" : ToLongDateString(_runningWorkers[eWorker]);

    public DateTime? GetLastRun(EWorkerNames eWorker)
    {
        if (_lastRun.TryGetValue(eWorker, out var run))
            return run;
        
        return null;
    }

    public void SetLastRun(EWorkerNames eWorker, DateTime? date = null)
    {
        date ??= DateTime.Now;

        if (_lastRun.ContainsKey(eWorker))
        {
            _lastRun[eWorker] = date.Value;
            return;
        }
        
        _lastRun.Add(eWorker, date.Value);
    }

    public object GetStatus()
    {
        return new
        {
            Configuration = new
            {
                WorkerEnabled = _workersEnabled
            },
            WorkersStatus = Enum.GetValues<EWorkerNames>()
                .ToDictionary(worker => worker.ToString(), IsWorkerRunning),
            UltimasExecucoes = _lastRun
                .ToDictionary(worker => worker.Key.ToString(),
                    worker => worker.Value.ToString(CultureInfo.CurrentCulture))
        };
    }
    
    private static string ToLongDateString(DateTime dateTime) => dateTime.ToString("dd-MM-yyyy HH:mm:ss");

    public override string ToString()
    {
        var status = GetStatus();
        return JsonConvert.SerializeObject(status);
    }
}

public enum EWorkerNames
{
    AtualizarAvisos = 1,
}