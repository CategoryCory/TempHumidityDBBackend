using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TempHumidityBackend.Handlers;

namespace TempHumidityBackend.Workers;

/// <summary>
/// A concrete implementation of the <see cref="IUDPListenerWorker"/> class.
/// </summary>
public class UDPListenerWorker : IUDPListenerWorker
{
    private readonly IConfiguration _config;
    private readonly ILogger<UDPListenerWorker> _logger;

    /// <summary>
    /// Constructs a new instance of the <see cref="UDPListenerWorker"/> class.
    /// </summary>
    /// <param name="config">The configuration object to use.</param>
    /// <param name="logger">The logger instance.</param>
    public UDPListenerWorker(IConfiguration config, ILogger<UDPListenerWorker> logger)
    {
        _config = config;
        _logger = logger;
    }

    /// <inheritdoc />
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
                await dataHandler.HandleData(receivedUdpResult.Buffer, stoppingToken);
            }
        }
        catch (SocketException e)
        {
            _logger.LogError("An error occurred when trying to establish socket:\n{}", e.Message);
            throw;
        }
    }
}