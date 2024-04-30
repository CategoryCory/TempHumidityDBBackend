using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TempHumidityBackend.Models;

namespace TempHumidityBackend.Services;

public class TempHumidityService : ITempHumidityService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TempHumidityService> _logger;

    public TempHumidityService(HttpClient httpClient, ILogger<TempHumidityService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

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
