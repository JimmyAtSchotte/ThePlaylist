using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using ThePlaylist.Infrastructure.NHibernate;

namespace ThePlaylist.WebApi.Tests;

public class Tests
{
    private CustomWebApplicationFactory _factory;
    private HttpClient _client;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new CustomWebApplicationFactory(services => services.AddNHibernate(hibernate =>
            hibernate.UseSqlLite()));

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