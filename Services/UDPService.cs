using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TempHumidityBackend.Data;
using TempHumidityBackend.Helpers;

namespace TempHumidityBackend;

public class UDPService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<UDPService> _logger;
    private readonly IServiceProvider _services;

    public UDPService(IServiceProvider services)
    {
        // _dbContext = dbContext;
        _services = services;

        _dbContext = _services.GetRequiredService<AppDbContext>();
        _logger = _services.GetRequiredService<ILogger<UDPService>>();
    }

    public async Task StartUDPListener(int listenPort)
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

                var decodedData = CborHelper.DecodeAHT20CborData(receivedBytes);

                if (decodedData.HasValue)
                {
                    var readingToAdd = Mappers.AHT20ReadingToTempHumidityModel(decodedData.Value!);

                    _dbContext.TempHumidities.Add(readingToAdd);
                    await _dbContext.SaveChangesAsync();

                    _logger.LogInformation("Message saved to database.");
                }
                else
                {
                    _logger.LogWarning("{}", decodedData.ErrorMessage);
                    _logger.LogWarning("No messages saved.");
                }
            }
        }
        catch (SocketException e)
        {
            _logger.LogError("An error occurred when trying to establish socket:\n{}", e.Message);
        }
    }
}
