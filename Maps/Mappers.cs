using TempHumidityBackend.Models;
using TempHumidityBackend.Types;

namespace TempHumidityBackend.Maps;

public static class Mappers
{
    public static TempHumidity AHT20ReadingToTempHumidityModel(AHT20Reading reading)
    {
        var epochTimestamp = Convert.ToInt64(reading.EpochTimestamp);
        var readAt = DateTimeOffset.FromUnixTimeSeconds(epochTimestamp).UtcDateTime;

        return new TempHumidity
        {
            TempCelsius = reading.TemperatureCelsius,
            Humidity = reading.RelativeHumidity,
            ReadAt = readAt
        };
    }
}
