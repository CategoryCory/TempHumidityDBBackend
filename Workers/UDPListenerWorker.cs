using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TempHumidityBackend.Handlers;

namespace TempHumidityBackend.Workers;

public class UDPListenerWorker : IUDPListenerWorker
{
    private readonly IConfiguration _config;
    private readonly ILogger<UDPListenerWorker> _logger;

    public UDPListenerWorker(IConfiguration config, ILogger<UDPListenerWorker> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task StartListener(CancellationToken stoppingToken, IDataHandler dataHandler)
    {
        int listenPort = _config.GetValue<int>("UDPPort");

        using var listener = new UdpClient(listenPort);

        var groupEndpoint = new IPEndPoint(IPAddress.Any, listenPort);
        _logger.LogInformation("Listening on port {}...", listenPort);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                byte[] receivedBytes = listener.Receive(ref groupEndpoint);

                _logger.LogInformation("Received {} bytes from {}", receivedBytes.Length, groupEndpoint);

                byte[] ackBytes = Encoding.ASCII.GetBytes("ACK");
                listener.Send(ackBytes, ackBytes.Length, groupEndpoint);

                _logger.LogInformation("ACK sent.");

                await dataHandler.HandleData(receivedBytes);
            }
        }
        catch (SocketException e)
        {
            _logger.LogError("An error occurred when trying to establish socket:\n{}", e.Message);
            throw;
        }
    }
}