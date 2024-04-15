using System.Formats.Cbor;

namespace TempHumidityBackend.Helpers;

static class CborHelper
{
    public static AHT20ReadingModel DecodeAHT20CborData(byte[] encodedBytes)
    {
        var cborReader = new CborReader(encodedBytes, CborConformanceMode.Lax);

        int? length = cborReader.ReadStartMap();

        // Read temperature
        var _ = cborReader.ReadTextString();
        var tempValue = cborReader.ReadSingle();

        // Read humidity
        _ = cborReader.ReadTextString();
        var humidityValue = cborReader.ReadSingle();

        // Read timestamp
        _ = cborReader.ReadTextString();
        var timestampValue = cborReader.ReadUInt64();

        cborReader.ReadEndMap();

        return new AHT20ReadingModel 
        {
            TemperatureCelsius = tempValue,
            RelativeHumidity = humidityValue,
            EpochTimestamp = timestampValue
        };
    }
}