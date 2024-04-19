using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;

namespace TempHumidityBackend;

public class UDPService : IUDPService
{
    private readonly ILogger<UDPService> _logger;

    public UDPService(ILogger<UDPService> logger)
    {
        _logger = logger;
    }

    public async Task StartUDPListener(int listenPort, IDataHandler _dataHandler)
    {
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
