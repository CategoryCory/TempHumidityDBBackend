namespace TempHumidityBackend.Handlers;

/// <summary>
/// An interface defining a CBOR data handler.
/// </summary>
public interface IDataHandler
{
    /// <summary>
    /// A method for handling CBOR data.
    /// </summary>
    /// <param name="data">The raw CBOR data to be handled.</param>
    /// <param name="cancellationToken">The cancellation token instance.</param>
    /// <returns></returns>
    Task HandleData(byte[] data, CancellationToken cancellationToken = default);
}
