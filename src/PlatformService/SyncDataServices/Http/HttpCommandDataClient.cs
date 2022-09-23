using System.Text;
using System.Text.Json;
using PlatformService.DTOs;

namespace PlatformService.SyncDataServices.Http;

public sealed class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    
    public async Task SendPlatformToCommand(PlatformReadDTO platformReadDTO)
    {
        var httpClient = new StringContent(
            JsonSerializer.Serialize(platformReadDTO), 
            Encoding.UTF8, 
            "application/json");

        var response = await _httpClient.PostAsync(_configuration["CommandService:PlatformsEndpoint"], httpClient);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "--> Sync POST to Command Service was OK!"
            : "--> Sync POST to Command Service was NOT OK!");
    }
}