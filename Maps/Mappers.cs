using TempHumidityBackend.Models;

namespace TempHumidityBackend;

public static class Mappers
{
    public static TempHumidity AHT20ReadingToTempHumidityModel(AHT20ReadingModel reading)
    {
        return new TempHumidity
        {
            TempC = reading.TemperatureCelsius,
            RelHumidity = reading.RelativeHumidity,
            Timestamp = Convert.ToInt64(reading.EpochTimestamp)
        };
    }
}
