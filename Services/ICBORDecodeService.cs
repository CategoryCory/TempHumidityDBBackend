using TempHumidityBackend.Types;

namespace TempHumidityBackend.Services;

public interface ICBORDecodeService<T> where T : class
{
    CBORDecodeResult<T> DecodeData(byte[] encodedBytes);
}
