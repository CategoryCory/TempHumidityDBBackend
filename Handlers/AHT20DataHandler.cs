
using Microsoft.Extensions.Logging;

namespace TempHumidityBackend.Handlers;

public class AHT20DataHandler : IDataHandler
{
    private readonly ICBORDecodeService _cborDecodeService;
    private readonly ITempHumidityService _tempHumidityService;
    private readonly ILogger<AHT20DataHandler> _logger;


    public AHT20DataHandler(ICBORDecodeService cborDecodeService, 
                            ITempHumidityService tempHumidityService,
                            ILogger<AHT20DataHandler> logger)
    {
        _cborDecodeService = cborDecodeService;
        _tempHumidityService = tempHumidityService;
        _logger = logger;
    }

    public async Task HandleData(byte[] data, CancellationToken cancellationToken = default)
    {
        var decodedData = _cborDecodeService.DecodeAHT20Data(data);

        if (decodedData.HasValue)
        {
            var readingToAdd = Mappers.AHT20ReadingToTempHumidityModel(decodedData.Value!);

            var saveSucceeded = await _tempHumidityService.AddNewReading(readingToAdd, cancellationToken);

            if (saveSucceeded)
            {
                _logger.LogInformation("Message successfully saved.");
            }
            else
            {
                _logger.LogWarning("Message save failed.");
            }
        }
        else
        {
            _logger.LogWarning("{}", decodedData.ErrorMessage);
            _logger.LogWarning("No messages saved.");
        }

    }
}
