
public class WindowsHostService : BackgroundService
{
    private readonly ILogger<WindowsHostService> _logger;

    public WindowsHostService(ILogger<WindowsHostService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("WindowsHostService running at: {time}", DateTimeOffset.Now);

                await Task.Delay(1000, stoppingToken);

                stoppingToken.Register(() => _logger.LogInformation("WindowsHostService is stopping."));

                while (!stoppingToken.IsCancellationRequested)
                {
                    //_logger.LogInformation("WindowsHostService is doing background work.");

                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }

                _logger.LogInformation("WindowsHostService has stopped.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);

            // Terminates this process and returns an exit code to the operating system.
            // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
            // performs one of two scenarios:
            // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
            // 2. When set to "StopHost": will cleanly stop the host, and log errors.
            //
            // In order for the Windows Service Management system to leverage configured
            // recovery options, we need to terminate the process with a non-zero exit code.
            Environment.Exit(1);
        }
    }
}
