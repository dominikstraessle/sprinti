namespace Sprinti.Api.Serial;

public class SerialWorker(ISerialAdapter serialAdapter, ILogger<SerialWorker> logger) : IHostedService, IDisposable
{
    private Timer _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Started SerialWorker");
        _timer = new Timer(_ => { serialAdapter.WriteLine("write"); }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Change(Timeout.Infinite, 0);
        logger.LogInformation("Stopped SerialWorker");

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        serialAdapter.Dispose();
    }
}