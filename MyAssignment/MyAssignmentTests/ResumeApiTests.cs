using System.Net;
using System.Net.Http.Json;
using API.Models;
using FluentAssertions;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MyAssignmentTests;

/// <summary>
/// Create a service that
/// can give back a list of resume when issuing a get request on the resume endpoint
/// can return the resume of a single user
/// must expose swagger endpoint
/// </summary>
public class ResumeApiTests
{
    [Fact]
    public async Task exposes_swagger_endpoint()
    {
        await using var server = CreateTestServerFactoryAsync();
        var client = server.CreateClient();
        var res = await client.GetAsync("/swagger");

        res.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task swagger_endpoint_servers_swagger_ui()
    {
        await using var server = CreateTestServerFactoryAsync();
        var client = server.CreateClient();
        var res = await client.GetAsync("/swagger");
        var data = await res.Content.ReadAsStringAsync();
        var html = new HtmlDocument();
        html.LoadHtml(data);
        html.DocumentNode.SelectSingleNode("//div[@id='swagger-ui']").Should().NotBeNull();
    }

    [Fact]
    public async Task the_resume_endPoint_exists()
    {
        await using var server = CreateTestServerFactoryAsync();
        var client = server.CreateClient();
        var res = await client.GetAsync("/resume");

        res.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task can_get_all_resumes()
    {
        await using var server = CreateTestServerFactoryAsync();
        var client = server.CreateClient();
        var res = await client.GetAsync("/resume");

        var resumes = await res.Content.ReadFromJsonAsync<List<Resume>>();

        resumes.Should().HaveCountGreaterThan(1);
    }

    [Theory]
    [InlineData("Diego")]
    [InlineData("Chris")]
    [InlineData("Juliet")]
    public async Task can_get_the_resume_for_a_specific_person(string user)
    {
        await using var server = CreateTestServerFactoryAsync();
       
        var client = server.CreateClient();
        var res = await client.GetAsync($"/resume?byUser={user}");
        
        var resumes = await res.Content.ReadFromJsonAsync<List<Resume>>();

        resumes.Should()
            .HaveCount(1)
            .And
            .ContainSingle(r => r.Name == user);
    }

    private static WebApplicationFactory<Program> CreateTestServerFactoryAsync()
    {
        var builder =  new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.ReplaceWithInMemoryDbContext<ResumeDb>();
            });
        });
;
        builder.CreateClient().GetAsync("/");
        return builder;
    }
}