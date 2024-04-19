using TempHumidityBackend.Types;

namespace TempHumidityBackend;

public interface ICBORDecodeService
{
    CBORDecodeResult<AHT20Reading> DecodeAHT20Data(byte[] encodedBytes);
}
