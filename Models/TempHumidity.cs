namespace TempHumidityBackend.Models;

/// <summary>
/// A class that represents data read from a temperature/humidity sensor.
/// </summary>
public partial class TempHumidity
{
    /// <summary>
    /// The temperature reading, in Celsius.
    /// </summary>
    public float TempCelsius { get; set; }

    /// <summary>
    /// The relative humidity reading.
    /// </summary>
    public float Humidity { get; set; }

    /// <summary>
    /// The timestamp of this reading.
    /// </summary>
    public DateTime ReadAt { get; set; }
}
