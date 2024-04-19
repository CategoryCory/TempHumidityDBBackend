using TempHumidityBackend.Data.Models;
using TempHumidityBackend.Types;

namespace TempHumidityBackend;

public static class Mappers
{
    public static TempHumidity AHT20ReadingToTempHumidityModel(AHT20Reading reading)
    {
        var epochTimestamp = Convert.ToInt64(reading.EpochTimestamp);
        var readAt = DateTimeOffset.FromUnixTimeSeconds(epochTimestamp).UtcDateTime;

        return new TempHumidity
        {
            TempC = reading.TemperatureCelsius,
            RelHumidity = reading.RelativeHumidity,
            ReadAt = readAt
        };
    }
}
