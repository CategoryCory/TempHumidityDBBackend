namespace TempHumidityBackend.Models;

public partial class TempHumidity
{
    public float TempCelsius { get; set; }

    public float Humidity { get; set; }

    public DateTime ReadAt { get; set; }
}
