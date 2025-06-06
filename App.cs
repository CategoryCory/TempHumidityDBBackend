using System.Net.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TempHumidityBackend.Handlers;
using TempHumidityBackend.Workers;

namespace TempHumidityBackend;

/// <summary>
/// A background service that listens for UDP packets, decodes them from CBOR,
/// and forwards the data to a REST API endpoint using <see cref="ITempHumidityService"/>. 
/// </summary>
class App : BackgroundService
{
    private readonly IDataHandler _dataHandler;
    private readonly IUDPListenerWorker _udpListener;
    private readonly ILogger<App> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    /// <param name="dataHandler">The data handler implementation.</param>
    /// <param name="udpListener">The UDP listener implementation.</param>
    /// <param name="logger">The logger instance.</param>
    public App([FromKeyedServices("aht20")] IDataHandler dataHandler,
               IUDPListenerWorker udpListener,
               ILogger<App> logger)
    {
        _dataHandler = dataHandler;
        _udpListener = udpListener;
        _logger = logger;
    }

    /// <inheritdoc />
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