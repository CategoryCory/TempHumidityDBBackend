﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using TempHumidityBackend.Handlers;
using TempHumidityBackend.Services;
using TempHumidityBackend.Workers;

namespace TempHumidityBackend;

class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddHostedService<App>();

        builder.Services.AddTransient<ICBORDecodeService, CBORDecodeService>();
        builder.Services.AddTransient<ITempHumidityService, TempHumidityService>();
        builder.Services.AddTransient<IUDPListenerWorker, UDPListenerWorker>();
        builder.Services.AddKeyedTransient<IDataHandler, AHT20DataHandler>("aht20");

        builder.Services.AddHttpClient<ITempHumidityService, TempHumidityService>(client => 
        {
            client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-Api-Key", builder.Configuration["ApiKey"]);
        });

        builder.Services.AddLogging(config => config.AddConsole());

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
