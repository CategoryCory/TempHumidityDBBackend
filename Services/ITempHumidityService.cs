using TempHumidityBackend.Models;

namespace TempHumidityBackend.Services;

/// <summary>
/// An interface for posting sensor data to a REST API.
/// </summary>
public interface ITempHumidityService
{
    /// <summary>
    /// Posts the given temperature and humidity data to a configured REST endpoint.
    /// </summary>
    /// <param name="tempHumidity">The temperature/humidity data to post.</param>
    /// <param name="cancellationToken">The cancellation token instance.</param>
    /// <returns>A task representing a boolean, indicating success/failure status.</returns>
    Task<bool> AddNewReading(TempHumidity tempHumidity, CancellationToken cancellationToken = default);
}
