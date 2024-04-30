namespace TempHumidityBackend.Handlers;

public interface IDataHandler
{
    Task HandleData(byte[] data, CancellationToken cancellationToken = default);
}
