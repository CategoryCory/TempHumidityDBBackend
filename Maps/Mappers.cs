using TempHumidityBackend.Models;

namespace TempHumidityBackend;

public static class Mappers
{
    public static TempHumidity AHT20ReadingToTempHumidityModel(AHT20ReadingModel reading)
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
