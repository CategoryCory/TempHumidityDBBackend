using Microsoft.Extensions.Configuration;

namespace TempHumidityBackend;

public class App
{
    private readonly IUDPService _udpService;
    private readonly IConfiguration _config;

    public App(IUDPService udpService, IConfiguration config)
    {
        _udpService = udpService;
        _config = config;
    }

    public async Task Run(string[] args)
    {
        int udpPort = _config.GetValue<int>("UDPPort");
        await _udpService.StartUDPListener(udpPort);
    }
}
