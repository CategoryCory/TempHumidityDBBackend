using TempHumidityBackend.Types;

namespace TempHumidityBackend;

public interface ICBORDecodeService<T> where T : class
{
    CBORDecodeResult<T> DecodeData(byte[] encodedBytes);
}
