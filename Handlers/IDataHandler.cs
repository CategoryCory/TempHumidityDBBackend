namespace TempHumidityBackend;

public interface IDataHandler
{
    Task HandleData(byte[] data);
}
