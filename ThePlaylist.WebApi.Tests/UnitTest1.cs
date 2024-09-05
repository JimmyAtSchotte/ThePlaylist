using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ThePlaylist.WebApi.Tests;

public class Tests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [SetUp]
    public void SetUp()
    {
        _client = _factory.CreateClient();
    }
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _factory.Dispose();
    }
    
    [Test]
    public async Task Test1()
    {
        var response = await _client.GetAsync("/api/genre");

        response.IsSuccessStatusCode.Should().BeTrue();
        response.Content.Headers.ContentType?.ToString().Should().Be("application/json; charset=utf-8");
    }
}