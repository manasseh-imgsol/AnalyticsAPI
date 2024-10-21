using System.Net.Http.Json;

namespace AnalyticsUI.Services;

public class AnalyticsService
{
    private readonly HttpClient _httpClient;

    public AnalyticsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<RequestLog>> GetRequestLogsAsync()
    {
        // Make an HTTP GET request to the API endpoint
        var response = await _httpClient.GetFromJsonAsync<List<RequestLog>>("/api/Analytics");
        return response ?? new List<RequestLog>();
    }
}

// Model matching the API's response
public class RequestLog
{
    public Guid Id { get; set; }
    public string Endpoint { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public bool IsSuccess { get; set; }
    public int TokensUsed { get; set; }
}
