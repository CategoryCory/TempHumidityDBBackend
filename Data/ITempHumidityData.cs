using TempHumidityBackend.Data.Models;

namespace TempHumidityBackend.Data;

public interface ITempHumidityData
{
    Task<int> Insert(TempHumidity tempHumidity);
}