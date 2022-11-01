using API.Models;

using FluentAssertions;
using HtmlAgilityPack;
using System.Net;
using System.Net.Http.Json;

namespace ResumeServiceApi.Tests;

public class ResumeApiTests
{
    [Fact]
    public async Task exposes_swagger_endpoint()
    {
        await using var server = ServiceFactory.CreateTestServerFactoryAsync();
        var client = server.CreateClient();
        var res = await client.GetAsync("/swagger");

        res.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task swagger_endpoint_servers_swagger_ui()
    {
        await using var server = ServiceFactory.CreateTestServerFactoryAsync();
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
        await using var server = ServiceFactory.CreateTestServerFactoryAsync();
        var client = server.CreateClient();
        var res = await client.GetAsync("/resume");

        res.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task can_get_all_resumes()
    {
        await using var server = ServiceFactory.CreateTestServerFactoryAsync();
        var client = server.CreateClient();
        var res = await client.GetAsync("/resume");

        var resumes = await res.Content.ReadFromJsonAsync<List<Resume>>();

        resumes.Should().HaveCountGreaterThan(1);
    }

    [Theory]
    [InlineData("Diego")]
    [InlineData("diego")]
    [InlineData("Chris")]
    [InlineData("Juliet")]
    public async Task can_get_the_resume_for_a_specific_person(string user)
    {
        await using var server = ServiceFactory.CreateTestServerFactoryAsync();

        var client = server.CreateClient();
        var res = await client.GetAsync($"/resume?byUser={user}");

        var resumes = await res.Content.ReadFromJsonAsync<List<Resume>>();

        resumes.Should()
            .HaveCount(1)
            .And
            .ContainSingle(r => StringComparer.OrdinalIgnoreCase.Equals(r.Name,user));
    }
}