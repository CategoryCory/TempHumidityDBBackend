namespace TempHumidityBackend.Types;

/// <summary>
/// A class representing data read from an AHT20 sensor.
/// </summary>
public sealed class AHT20Reading
{
    /// <summary>
    /// The temperature reading, in Celsius.
    /// </summary>
    public float TemperatureCelsius { get; init; }

    /// <summary>
    /// The relative humidity reading.
    /// </summary>
    public float RelativeHumidity { get; init; }

    /// <summary>
    /// The timestamp of this reading.
    /// </summary>
    public UInt64 EpochTimestamp { get; init; }
}
