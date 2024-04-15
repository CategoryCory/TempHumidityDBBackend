using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TempHumidityBackend.Data;
using TempHumidityBackend.Services;

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
            Console.WriteLine(e.Message);
        }
    }

    static IHostBuilder CreateHostBuilder(string[] str)
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((builder, services) => 
            {
                services.AddSingleton<ITempHumidityService, TempHumidityService>();
                services.AddSingleton<App>();
                services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("ApplicationConnection")));
            })
            .ConfigureAppConfiguration(app => 
            {
                app.AddJsonFile("appsettings.json");
            });
    }
}
