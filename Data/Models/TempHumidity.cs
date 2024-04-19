namespace TempHumidityBackend.Data.Models;

public partial class TempHumidity
{
    public int Id { get; set; }

    public float? TempC { get; set; }

    public float? RelHumidity { get; set; }

    public DateTime? ReadAt { get; set; }
}
