using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TempHumidityBackend.Data;

namespace TempHumidityBackend;

class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;

        try
        {
            await services.GetRequiredService<App>().Run(args);
        }
        catch (Exception e)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError("{}", e.Message);
        }
    }

    static IHostBuilder CreateHostBuilder(string[] str)
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((builder, services) => 
            {
                services.AddSingleton<App>();
                services.AddLogging(config => config.AddConsole());
                services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("ApplicationConnection")));
                services.AddTransient<IUDPService, UDPService>();
            })
            .ConfigureAppConfiguration(app => 
            {
                app.AddJsonFile("appsettings.json");
            });
    }
}
