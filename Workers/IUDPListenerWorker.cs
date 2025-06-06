using TempHumidityBackend.Handlers;

namespace TempHumidityBackend.Workers;

/// <summary>
/// An interface defining a listener worker for UDP connections.
/// </summary>
public interface IUDPListenerWorker
{
    /// <summary>
    /// Starts the listener.
    /// </summary>
    /// <param name="stoppingToken">The cancellation token instance.</param>
    /// <param name="dataHandler">An instance of a data handler for received data.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task StartListener(CancellationToken stoppingToken, IDataHandler dataHandler);
}