using System.Formats.Cbor;
using TempHumidityBackend.Types;

namespace TempHumidityBackend;

public class CBORDecodeService : ICBORDecodeService
{
    public CBORDecodeService()
    {
    }

    public CBORDecodeResult<AHT20Reading> DecodeAHT20Data(byte[] encodedBytes)
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

            var result = new AHT20Reading 
            {
                TemperatureCelsius = tempValue,
                RelativeHumidity = humidityValue,
                EpochTimestamp = timestampValue
            };

            return CBORDecodeResult<AHT20Reading>.Success(result);
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

            return CBORDecodeResult<AHT20Reading>.Error(errorMessage);
        }
    }
}
