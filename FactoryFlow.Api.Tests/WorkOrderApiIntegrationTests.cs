using System.Net;
using System.Net.Http.Json;
using FactoryFlow.Api.Dtos;
using FactoryFlow.Api.Models;

namespace FactoryFlow.Api.Tests;

public class WorkOrderApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public WorkOrderApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateAndStartWorkOrder_UsesHttpPipelineAndPersistsStatusChange()
    {
        HttpResponseMessage createResponse = await _client.PostAsJsonAsync("/api/workorders", new CreateWorkOrderRequest
        {
            MachineId = 1,
            Title = "Repair spindle",
            Description = "Spindle has abnormal vibration."
        });

        WorkOrderResponse? created = await createResponse.Content.ReadFromJsonAsync<WorkOrderResponse>();

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        Assert.NotNull(created);
        Assert.Equal(1, created.MachineId);
        Assert.Equal("Repair spindle", created.Title);
        Assert.Equal(WorkOrderStatus.Open, created.Status);
        Assert.Null(created.CompletedAt);

        HttpResponseMessage startResponse = await _client.PostAsync($"/api/workorders/{created.Id}/start", null);
        WorkOrderResponse? started = await startResponse.Content.ReadFromJsonAsync<WorkOrderResponse>();

        Assert.Equal(HttpStatusCode.OK, startResponse.StatusCode);
        Assert.Equal(WorkOrderStatus.InProgress, started?.Status);
    }

    [Fact]
    public async Task Create_WhenMachineDoesNotExist_ReturnsNotFoundProblem()
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/workorders", new CreateWorkOrderRequest
        {
            MachineId = 999,
            Title = "Repair missing machine",
            Description = "This machine does not exist."
        });

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
