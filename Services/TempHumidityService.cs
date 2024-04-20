using Microsoft.Extensions.Logging;
using TempHumidityBackend.Data;
using TempHumidityBackend.Data.Models;

namespace TempHumidityBackend.Services;

public class TempHumidityService : ITempHumidityService
{
    private readonly ITempHumidityData _tempHumidityData;
    private readonly ILogger<TempHumidityService> _logger;

    public TempHumidityService(ITempHumidityData tempHumidityData, ILogger<TempHumidityService> logger)
    {
        _tempHumidityData = tempHumidityData;
        _logger = logger;
    }

    public async Task<bool> AddNewReading(TempHumidity tempHumidity)
    {
        try
        {
            int insertedRows = await _tempHumidityData.Insert(tempHumidity);

            return insertedRows == 1;
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while saving to the database:\n{}", ex.Message);

            return false;
        }
    }
}
