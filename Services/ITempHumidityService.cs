using TempHumidityBackend.Models;

namespace TempHumidityBackend;

public interface ITempHumidityService
{
    Task<bool> AddNewReading(TempHumidity tempHumidity, CancellationToken cancellationToken = default);
}
