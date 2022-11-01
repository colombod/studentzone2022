using Microsoft.OpenApi.Models;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Resumes") ?? "Data Source=Resumes.db";

builder.Services.AddSqlite<ResumeDb>(connectionString);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Resume API", Description = "an API for a resume", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Resume API V1");
});

app.MapGet("/", (ResumeDb db) =>
{
    db.Database.EnsureCreated();
    return "Hello World!";
});

app.MapGet("/resume", GetResumes);

List<Resume> GetResumes(ResumeDb resumeDb, string? byUser)
{
    IQueryable<Resume> resumes = resumeDb.Resumes;
    if (!string.IsNullOrEmpty(byUser))
    {
        resumes = resumeDb.Resumes.Where(r => StringComparer.OrdinalIgnoreCase.Equals(r.Name, byUser));
    }

    var ret = resumes
        .Include(r => r.Skills)
        .Include(r => r.Educations)
        .Include(r => r.Experiences)
        .ToList();
    
    return ret;
}

app.Run();


