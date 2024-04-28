namespace TempHumidityBackend.Handlers;

public interface IDataHandler
{
    Task HandleData(byte[] data);
}
