using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TempHumidityBackend.Data;
using TempHumidityBackend.Services;

namespace TempHumidityBackend;

class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddHostedService<UDPListenerWorker>();
        builder.Services.AddLogging(config => config.AddConsole());
        builder.Services.AddTransient<ITempHumidityData, TempHumidityData>();
        builder.Services.AddTransient<ICBORDecodeService, CBORDecodeService>();
        builder.Services.AddTransient<ITempHumidityService, TempHumidityService>();
        builder.Services.AddKeyedTransient<IDataHandler, AHT20DataHandler>("aht20");

        var host = builder.Build();

        try
        {
            host.Run();    
        }
        catch (Exception e)
        {
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError("{}", e.Message);
        }
    }
}
