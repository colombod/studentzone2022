# setup

## setup tests 
```csharp
namespace ResumeServiceApi.Tests;

public class ResumeApiTests
{
    [Fact]
    public void exposes_swagger_endpoint()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void swagger_endpoint_servers_swagger_ui()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void the_resume_endPoint_exists()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void can_get_all_resumes()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void can_get_the_resume_for_a_specific_person()
    {
        throw new NotImplementedException();
    }
}
```

## final shape
```csharp
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
```

```csharp
using Microsoft.OpenApi.Models;
using API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Resumes") ?? "Data Source=Resumes.db";

builder.Services.AddSqlite<ResumeDb>(connectionString);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Resume API", Description = "an API for a resume", Version = "v1" });
});

var app = builder.Build();

app.MapGet("/", (ResumeDb db) =>
{
    db.Database.EnsureCreated();
    return "Hello World!";
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Resume API V1");
});

app.MapGet("/resume", GetResumes);

List<Resume> GetResumes(ResumeDb resumeDb)
{
    
    return new List<Resume>();
}

List<Resume> GetResumes(ResumeDb resumeDb)
{    
    var ret = resumes = resumeDb.Resumes
        .Include(r => r.Skills)
        .Include(r => r.Educations)
        .Include(r => r.Experiences)
        .ToList();
    return ret;
}


List<Resume> GetResumes(ResumeDb resumeDb, string? byUser)
{
    IQueryable<Resume> resumes = resumeDb.Resumes;
    if (!string.IsNullOrEmpty(byUser))
    {
        resumes = resumeDb.Resumes.Where(r => r.Name == byUser);
    }

    var ret = resumes
        .Include(r => r.Skills)
        .Include(r => r.Educations)
        .Include(r => r.Experiences)
        .ToList();
    return ret;
}

app.Run();
```


```csharp
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public class DbInitializer 
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Resume>().HasData(
            new Resume { Id = 1, Name = "Chris" }
        );

        modelBuilder.Entity<Skill>().HasData(
            new Skill { Id = 1, Name = ".NET", EstimatedDescription = ".NET", EstimatedLevel = 5, ResumeId = 1 }
        );
        modelBuilder.Entity<Education>().HasData(
            new Education { Id = 1, Name = "BSC Computer Science", DegreeYear = "2020", Description = "Bachelor in computer science", ResumeId = 1 }
        );

        modelBuilder.Entity<Experience>().HasData(
            new Experience { Id = 1, CompanyName = "Microsoft", Tenure = "2020-2022", Title = "Developer", Description = "Developer in a team of 5", ResumeId = 1 }
        );
        
        modelBuilder.Entity<Resume>().HasData(
            new Resume { Id = 2, Name = "Diego" }
        );

        modelBuilder.Entity<Skill>().HasData(
            new Skill { Id = 2, Name = ".NET", EstimatedDescription = ".NET", EstimatedLevel = 5, ResumeId = 2 }
        );
        modelBuilder.Entity<Education>().HasData(
            new Education { Id = 2, Name = "BSC Computer Science", DegreeYear = "2020", Description = "Bachelor in computer science", ResumeId = 2 }
        );

        modelBuilder.Entity<Experience>().HasData(
            new Experience { Id = 2, CompanyName = "Microsoft", Tenure = "2020-2022", Title = "Developer", Description = "Developer in a team of 5", ResumeId = 2 }
        );

        modelBuilder.Entity<Resume>().HasData(
            new Resume { Id = 3, Name = "Juliet" }
        );

        modelBuilder.Entity<Skill>().HasData(
            new Skill { Id = 3, Name = ".NET", EstimatedDescription = ".NET", EstimatedLevel = 5, ResumeId = 3 }
        );
        modelBuilder.Entity<Education>().HasData(
            new Education { Id = 3, Name = "MSC Computer Science", DegreeYear = "2021", Description = "Master Degree in computer science", ResumeId = 3 }
        );

        modelBuilder.Entity<Experience>().HasData(
            new Experience { Id = 3, CompanyName = "Microsoft", Tenure = "2021-2022", Title = "Developer", Description = "Developer in a team of 5", ResumeId = 23 }
        );
    }
}
```