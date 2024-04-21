using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TempHumidityBackend;

public class UDPListenerWorker : BackgroundService
{
    private readonly IDataHandler _dataHandler;
    private readonly IConfiguration _config;
    private readonly ILogger<UDPListenerWorker> _logger;

    public UDPListenerWorker([FromKeyedServices("aht20")] IDataHandler dataHandler,
                             IConfiguration config,
                             ILogger<UDPListenerWorker> logger)
    {
        _dataHandler = dataHandler;
        _config = config;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            int listenPort = _config.GetValue<int>("UDPPort");

            using var listener = new UdpClient(listenPort);

            var groupEndpoint = new IPEndPoint(IPAddress.Any, listenPort);
            _logger.LogInformation("Listening on port {}...", listenPort);

            try
            {
                while (true)
                {
                    byte[] receivedBytes = listener.Receive(ref groupEndpoint);

                    _logger.LogInformation("Received {} bytes from {}", receivedBytes.Length, groupEndpoint);

                    byte[] ackBytes = Encoding.ASCII.GetBytes("ACK");
                    listener.Send(ackBytes, ackBytes.Length, groupEndpoint);

                    _logger.LogInformation("ACK sent.");

                    await _dataHandler.HandleData(receivedBytes);
                }
            }
            catch (SocketException e)
            {
                _logger.LogError("An error occurred when trying to establish socket:\n{}", e.Message);
            }
        }
    }
}