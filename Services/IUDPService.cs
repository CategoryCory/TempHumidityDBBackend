namespace TempHumidityBackend;

public interface IUDPService
{
    Task StartUDPListener(int listenPort);
}
