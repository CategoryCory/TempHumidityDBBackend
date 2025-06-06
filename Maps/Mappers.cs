using TempHumidityBackend.Models;
using TempHumidityBackend.Types;

namespace TempHumidityBackend.Maps;

/// <summary>
/// A class containing mappers to be used in the TempHumidityBackend project.
/// </summary>
public static class Mappers
{
    /// <summary>
    /// Convert AHT20 sensor reading to an instance of <see cref="TempHumidity"/>. 
    /// </summary>
    /// <param name="reading">The AHT20 reading to convert.</param>
    /// <returns>The mapped <see cref="TempHumidyt"/> object.</returns>
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
