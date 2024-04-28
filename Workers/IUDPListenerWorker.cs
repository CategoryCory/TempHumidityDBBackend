using TempHumidityBackend.Handlers;

namespace TempHumidityBackend.Workers;

public interface IUDPListenerWorker
{
    Task StartListener(CancellationToken stoppingToken, IDataHandler dataHandler);
}