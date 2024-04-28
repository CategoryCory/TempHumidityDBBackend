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
        ReadOnlyMemory<byte> ackBytes = Encoding.ASCII.GetBytes("ACK");

        using var listener = new UdpClient(listenPort);

        var groupEndpoint = new IPEndPoint(IPAddress.Any, listenPort);
        _logger.LogInformation("Listening on port {}...", listenPort);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Listen for UDP message
                var receivedUdpResult = await listener.ReceiveAsync(stoppingToken);
                _logger.LogInformation("Received {} bytes from {}", receivedUdpResult.Buffer.Length, groupEndpoint);

                // Send acknowledgement when message is received
                await listener.SendAsync(ackBytes, receivedUdpResult.RemoteEndPoint, stoppingToken);
                _logger.LogInformation("ACK sent.");

                // Call data handler for received message
                await dataHandler.HandleData(receivedUdpResult.Buffer);
            }
        }
        catch (SocketException e)
        {
            _logger.LogError("An error occurred when trying to establish socket:\n{}", e.Message);
            throw;
        }
    }
}