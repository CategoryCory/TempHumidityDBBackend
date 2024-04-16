using System.Net;
using System.Net.Sockets;
using System.Text;
using TempHumidityBackend.Data;
using TempHumidityBackend.Helpers;

namespace TempHumidityBackend;

public static class UDPService
{
    public static async Task StartUDPListener(int listenPort, AppDbContext dbContext)
    {
        using var listener = new UdpClient(listenPort);

        var groupEndpoint = new IPEndPoint(IPAddress.Any, listenPort);
        Console.WriteLine($"Listening on port {listenPort}...");

        try
        {
            while (true)
            {
                byte[] receivedBytes = listener.Receive(ref groupEndpoint);

                Console.WriteLine($"Received {receivedBytes.Length} bytes from {groupEndpoint}");

                byte[] ackBytes = Encoding.ASCII.GetBytes("ACK");
                listener.Send(ackBytes, ackBytes.Length, groupEndpoint);

                Console.WriteLine("ACK sent.");

                var decodedData = CborHelper.DecodeAHT20CborData(receivedBytes);

                if (decodedData.HasValue)
                {
                    var readingToAdd = Mappers.AHT20ReadingToTempHumidityModel(decodedData.Value!);

                    dbContext.TempHumidities.Add(readingToAdd);
                    await dbContext.SaveChangesAsync();

                    Console.WriteLine("Message saved to database.\n");
                }
                else
                {
                    Console.WriteLine(decodedData.ErrorMessage);
                    Console.WriteLine("No data saved to database.");
                }
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine($"An error occurred when trying to establish socket:\n{e.Message}");
        }
    }
}
