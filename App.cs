using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TempHumidityBackend;

public class App
{
    private readonly IUDPService _udpService;
    private readonly IConfiguration _config;
    private readonly IDataHandler _dataHandler;

    public App(IUDPService udpService, IConfiguration config, [FromKeyedServices("aht20")] IDataHandler dataHandler)
    {
        _udpService = udpService;
        _config = config;
        _dataHandler = dataHandler;
    }

    public async Task Run(string[] args)
    {
        int udpPort = _config.GetValue<int>("UDPPort");
        await _udpService.StartUDPListener(udpPort, _dataHandler);
    }
}
