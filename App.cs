using Microsoft.Extensions.Configuration;
using TempHumidityBackend.Data;

namespace TempHumidityBackend;

public class App
{
    private readonly IConfiguration _config;
    private readonly IServiceProvider _services;

    public App(IConfiguration config, IServiceProvider services)
    {
        _config = config;
        _services = services;
    }

    public async Task Run(string[] args)
    {
        int udpPort = _config.GetValue<int>("UDPPort");
        var udpService = new UDPService(_services);
        await udpService.StartUDPListener(udpPort);
    }
}
