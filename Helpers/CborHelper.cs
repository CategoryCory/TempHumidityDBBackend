using System.Formats.Cbor;

namespace TempHumidityBackend.Helpers;

static class CborHelper
{
    public static CBORDecodeResult<AHT20ReadingModel> DecodeAHT20CborData(byte[] encodedBytes)
    {
        try
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

            var result = new AHT20ReadingModel 
            {
                TemperatureCelsius = tempValue,
                RelativeHumidity = humidityValue,
                EpochTimestamp = timestampValue
            };

            return CBORDecodeResult<AHT20ReadingModel>.Success(result);
        }
        catch (Exception ex)
        {
            string errorMessage = ex switch
            {
                ArgumentOutOfRangeException => $"An argument was out of range when deserializing CBOR data:\n{ex.Message}",
                InvalidOperationException => $"An invalid operation occurred while deserializing CBOR data:\n{ex.Message}",
                OverflowException => $"An overflow exception occurred while deserializing CBOR data:\n{ex.Message}",
                CborContentException => $"A CBOR content error occurred while deserializing CBOR data:\n{ex.Message}",
                _ => $"An error occurred while deserializing CBOR data:\n{ex.Message}"
            };

            return CBORDecodeResult<AHT20ReadingModel>.Error(errorMessage);
        }
    }
}