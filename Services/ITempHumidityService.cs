using TempHumidityBackend.Data.Models;

namespace TempHumidityBackend;

public interface ITempHumidityService
{
    Task<bool> AddNewReading(TempHumidity tempHumidity);
}
