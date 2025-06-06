using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TempHumidityBackend.Models;

namespace TempHumidityBackend.Services;

/// <summary>
/// An implementation of <see cref="ITempHumidityService"/> that sends data via HTTP POST to a REST API endpoint.
/// </summary>
public class TempHumidityService : ITempHumidityService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TempHumidityService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TempHumidityService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client to use for sending requests.</param>
    /// <param name="logger">The logger instance.</param>
    public TempHumidityService(HttpClient httpClient, ILogger<TempHumidityService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<bool> AddNewReading(TempHumidity tempHumidity, CancellationToken cancellationToken = default)
    {
        var serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };

        try
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync("sensors/", tempHumidity, serializerOptions, cancellationToken);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("{}", jsonResponse);

            return true;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("An error occurred while saving data:\n{}", ex.Message);
            return false;
        }
    }
}
