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