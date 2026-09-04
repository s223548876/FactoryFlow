using System.Net;
using System.Net.Http.Json;

namespace FactoryFlow.Api.Tests;

public class MachineApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public MachineApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task WeatherForecastEndpoint_ReturnsNotFound()
    {
        HttpResponseMessage response = await _client.GetAsync("/weatherforecast");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Start_WhenMachineDoesNotExist_ReturnsNotFoundProblem()
    {
        HttpResponseMessage response = await _client.PostAsync("/api/machines/UNKNOWN/start", null);
        ProblemDetailsResponse? body = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("找不到指定的 Machine", body?.Title);
    }

    private class ProblemDetailsResponse
    {
        public string? Title { get; set; }
        public int? Status { get; set; }
    }
}
