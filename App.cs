using System.Net.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TempHumidityBackend.Handlers;
using TempHumidityBackend.Workers;

namespace TempHumidityBackend;

class App : BackgroundService
{
    private readonly IDataHandler _dataHandler;
    private readonly IUDPListenerWorker _udpListener;
    private readonly ILogger<App> _logger;

    public App([FromKeyedServices("aht20")] IDataHandler dataHandler,
               IUDPListenerWorker udpListener,
               ILogger<App> logger)
    {
        _dataHandler = dataHandler;
        _udpListener = udpListener;
        _logger = logger;    
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _udpListener.StartListener(stoppingToken, _dataHandler);
        }
        catch (SocketException ex)
        {
            _logger.LogError("An error occurred while executing the service worker:{}\n", ex.Message);
        }
    }
}