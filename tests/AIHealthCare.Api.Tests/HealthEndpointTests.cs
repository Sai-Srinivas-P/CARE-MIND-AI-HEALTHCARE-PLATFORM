namespace AIHealthCare.Api.Tests;
using Microsoft.AspNetCore.Mvc.Testing;

public sealed class HealthEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;
    public HealthEndpointTests(WebApplicationFactory<Program> factory) => this.factory = factory;

    [Fact]
    public async Task HealthEndpoint_ShouldReturnSuccess()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/api/health");
        Assert.True(response.IsSuccessStatusCode);
    }
}
