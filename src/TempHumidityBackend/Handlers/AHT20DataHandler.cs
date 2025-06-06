
using Microsoft.Extensions.Logging;
using TempHumidityBackend.Maps;
using TempHumidityBackend.Services;
using TempHumidityBackend.Types;

namespace TempHumidityBackend.Handlers;

/// <summary>
/// A concrete implementation of <see cref="IDataHandler"/> for handling AHT20 data. 
/// </summary>
public class AHT20DataHandler : IDataHandler
{
    private readonly ICBORDecodeService<AHT20Reading> _cborDecodeService;
    private readonly ITempHumidityService _tempHumidityService;
    private readonly ILogger<AHT20DataHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AHT20DataHandler"/> class.
    /// </summary>
    /// <param name="cborDecodeService">The CBOR decode service instance.</param>
    /// <param name="tempHumidityService">The temperature/humidity service instance.</param>
    /// <param name="logger">The logger instance.</param>
    public AHT20DataHandler(ICBORDecodeService<AHT20Reading> cborDecodeService,
                            ITempHumidityService tempHumidityService,
                            ILogger<AHT20DataHandler> logger)
    {
        _cborDecodeService = cborDecodeService;
        _tempHumidityService = tempHumidityService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task HandleData(byte[] data, CancellationToken cancellationToken = default)
    {
        var decodedData = _cborDecodeService.DecodeData(data);

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
