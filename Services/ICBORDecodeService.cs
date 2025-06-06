using TempHumidityBackend.Types;

namespace TempHumidityBackend.Services;

/// <summary>
/// An interface defining the service for decoding CBOR data.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICBORDecodeService<T> where T : class
{
    /// <summary>
    /// Decodes CBOR data.
    /// </summary>
    /// <param name="encodedBytes">The encoded CBOR data to decode.</param>
    /// <returns>A <see cref="CBORDecodeResult"/> object containing the decoded data.</returns>
    CBORDecodeResult<T> DecodeData(byte[] encodedBytes);
}
