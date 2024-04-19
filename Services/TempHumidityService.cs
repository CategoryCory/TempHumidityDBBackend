using Microsoft.Extensions.Logging;
using TempHumidityBackend.Data;
using TempHumidityBackend.Data.Models;

namespace TempHumidityBackend.Services;

public class TempHumidityService : ITempHumidityService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<TempHumidityService> _logger;

    public TempHumidityService(AppDbContext dbContext, ILogger<TempHumidityService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> AddNewReading(TempHumidity tempHumidity)
    {
        try
        {
            _dbContext.TempHumidities.Add(tempHumidity);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while saving to the database:\n{}", ex.Message);

            return false;
        }
    }
}
