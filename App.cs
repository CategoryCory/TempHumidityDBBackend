using Microsoft.Extensions.Configuration;
using TempHumidityBackend.Data;
using TempHumidityBackend.Services;

namespace TempHumidityBackend;

public class App
{
    private readonly IConfiguration _config;
    private readonly ITempHumidityService _tempHumidityService;
    private readonly AppDbContext _dbContext;

    public App(IConfiguration config, ITempHumidityService tempHumidityService, AppDbContext dbContext)
    {
        _config = config;
        _tempHumidityService = tempHumidityService;
        _dbContext = dbContext;
    }

    public async Task Run(string[] args)
    {
        int udpPort = _config.GetValue<int>("UDPPort");
        await UDPService.StartUDPListener(udpPort, _dbContext);
    }
}
