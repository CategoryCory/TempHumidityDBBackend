namespace TempHumidityBackend;

public sealed class AHT20ReadingModel
{
    public float TemperatureCelsius { get; init; }
    public float RelativeHumidity { get; init; }
    public UInt64 EpochTimestamp { get; init; }
}
