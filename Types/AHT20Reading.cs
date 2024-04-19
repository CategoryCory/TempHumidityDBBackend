namespace TempHumidityBackend.Types;

public sealed class AHT20Reading
{
    public float TemperatureCelsius { get; init; }
    public float RelativeHumidity { get; init; }
    public UInt64 EpochTimestamp { get; init; }
}
