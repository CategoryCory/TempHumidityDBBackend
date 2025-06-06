using TempHumidityBackend.Models;

namespace TempHumidityBackend.Services;

public interface ITempHumidityService
{
    Task<bool> AddNewReading(TempHumidity tempHumidity, CancellationToken cancellationToken = default);
}
