using System.Text.Json;
using System.Threading.Tasks.Dataflow;
using LearnToDev.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnToDev.Api.Data;

// Fills the database with game content the first time the app starts
public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext db, ILogger logger, string seedFolder)
    {
        // 1. Do nothing if the database already has content
        if (await db.Schools.AnyAsync())
        {
            logger.LogInformation("Database already has content, skipping seed.");
            return;
        }

        // 2. Housings, numbers from docs/spilltall.md (+2 hours per tier)
        db.Housings.AddRange(
            new Housing { Name = "Telt", Order = 1, HoursPerDay = 12, Rent = 0 },
            new Housing { Name = "Dorm", Order = 2, HoursPerDay = 14, Rent = 40 },
            new Housing { Name = "Hybel", Order = 3, HoursPerDay = 16, Rent = 80 },
            new Housing { Name = "Leilighet", Order = 4, HoursPerDay = 18, Rent = 160 },
            new Housing { Name = "Penthouse", Order = 5, HoursPerDay = 20, Rent = 300 }
        );
        await db.SaveChangesAsync();

        // 3. Schools, levels, questions and answer options from the JSON files
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        foreach (var file in Directory.GetFiles(seedFolder, "*.json"))
        {
            try
            {
                await using var stream = File.OpenRead(file);
                var school = await JsonSerializer.DeserializeAsync<School>(stream, options);

                if (school == null)
                {
                    logger.LogWarning("Seed file {File} was empty and was skipped", file);
                    continue;
                }

                db.Schools.Add(school);
            }
            catch (Exception ex) when (ex is JsonException or IOException)
            {
                logger.LogError(ex, "Could not read seed file {File}.", file);
            }
        }
        await db.SaveChangesAsync();

        // 4. Jobs, numbers from docs/spilltall.md
        var itHelp = new Job { Name = "IT Help", Rank = 1, Wage = 20, HoursPerShift = 4, TasksPerDay = GameRules.TasksPerWorkDay };
        var studFrontend = new Job { Name = "Stud.ass Frontend", Rank = 2, Wage = 75, HoursPerShift = 4, TasksPerDay = GameRules.TasksPerWorkDay };
        var studBackend = new Job { Name = "Stud.ass Backend", Rank = 2, Wage = 75, HoursPerShift = 4, TasksPerDay = GameRules.TasksPerWorkDay };
        var studDatabase = new Job { Name = "Stud.ass Database", Rank = 2, Wage = 75, HoursPerShift = 4, TasksPerDay = GameRules.TasksPerWorkDay };
        var office = new Job { Name = "Office", Rank = 3, Wage = 150, HoursPerShift = 6, TasksPerDay = GameRules.TasksPerWorkDay };
        var startup = new Job { Name = "Startup", Rank = 4, Wage = 260, HoursPerShift = 6, TasksPerDay = GameRules.TasksPerWorkDay };
        var techCorp = new Job { Name = "Tech Corp", Rank = 5, Wage = 450, HoursPerShift = 8, TasksPerDay = GameRules.TasksPerWorkDay };

        db.Jobs.AddRange(itHelp, studFrontend, studBackend, studDatabase, office, startup, techCorp);

        // Tasks the player gets at work, with the answers that count as correct

        // IT Help
        itHelp.JobTasks.Add(new JobTask
        {
            Text = "En bruker får ikke koblet til Wi-Fi. Hva ber du dem gjøre først?",
            JobTaskAnswers = new List<JobTaskAnswer>
            {
                new JobTaskAnswer { Text = "restart" },
                new JobTaskAnswer { Text = "start på nytt" }
            }
        });

        // Stud.ass Frontend
        studFrontend.JobTasks.Add(new JobTask
        {
            Text = "Hvilken HTML-tag lager en lenke?",
            JobTaskAnswers = new List<JobTaskAnswer>
            {
                new JobTaskAnswer { Text = "a" },
                new JobTaskAnswer { Text = "<a>" }
            }
        });

        // Stud.ass Backend
        studBackend.JobTasks.Add(new JobTask
        {
            Text = "Hvilket nøkkelord bruker du i C# for å vente på en asynkron metode?",
            JobTaskAnswers = new List<JobTaskAnswer>
            {
                new JobTaskAnswer { Text = "await" }
            }
        });

        // Stud.ass Database
        studDatabase.JobTasks.Add(new JobTask
        {
            Text = "Hvilken SQL-kommando henter data fra en tabell?",
            JobTaskAnswers = new List<JobTaskAnswer>
            {
                new JobTaskAnswer { Text = "SELECT" }
            }
        });

        // Office
        office.JobTasks.Add(new JobTask
        {
            Text = "Hvilken HTTP-metode brukes for å opprette en ny ressurs i et API?",
            JobTaskAnswers = new List<JobTaskAnswer>
            {
                new JobTaskAnswer { Text = "POST" }
            }
        });

        // Startup
        startup.JobTasks.Add(new JobTask
        {
            Text = "Hvilken HTTP-statuskode betyr «Not Found»?",
            JobTaskAnswers = new List<JobTaskAnswer>
            {
                new JobTaskAnswer { Text = "404" }
            }
        });

        // Tech Corp
        techCorp.JobTasks.Add(new JobTask
        {
            Text = "Hva kalles mønsteret der en klasse får avhengighetene sine utenfra via konstruktøren?",
            JobTaskAnswers = new List<JobTaskAnswer>
            {
                new JobTaskAnswer { Text = "dependency injection" },
                new JobTaskAnswer { Text = "DI" }
            }
        });

        // Look up the school levels the jobs require (saved in step 3)
        var frontend1 = await db.SchoolLevels.SingleAsync(l => l.School.Name == "Frontend" && l.Level == 1);
        var backend1 = await db.SchoolLevels.SingleAsync(l => l.School.Name == "Backend" && l.Level == 1);
        var database1 = await db.SchoolLevels.SingleAsync(l => l.School.Name == "Database" && l.Level == 1);
        var fullstack1 = await db.SchoolLevels.SingleAsync(l => l.School.Name == "Fullstack" && l.Level == 1);
        var fullstack2 = await db.SchoolLevels.SingleAsync(l => l.School.Name == "Fullstack" && l.Level == 2);
        var fullstack3 = await db.SchoolLevels.SingleAsync(l => l.School.Name == "Fullstack" && l.Level == 3);

        // Which school level each job requires (IT Help requires nothing)
        studFrontend.JobRequirements.Add(new JobRequirement { SchoolLevel = frontend1 });
        studBackend.JobRequirements.Add(new JobRequirement { SchoolLevel = backend1 });
        studDatabase.JobRequirements.Add(new JobRequirement { SchoolLevel = database1 });
        office.JobRequirements.Add(new JobRequirement { SchoolLevel = fullstack1 });
        startup.JobRequirements.Add(new JobRequirement { SchoolLevel = fullstack2 });
        techCorp.JobRequirements.Add(new JobRequirement { SchoolLevel = fullstack3 });

        // Which job the player must have had before (the job ladder)
        studFrontend.Prerequisites.Add(new JobPrerequisite { RequiredJob = itHelp });
        studBackend.Prerequisites.Add(new JobPrerequisite { RequiredJob = itHelp });
        studDatabase.Prerequisites.Add(new JobPrerequisite { RequiredJob = itHelp });
        office.Prerequisites.Add(new JobPrerequisite { RequiredJob = studFrontend });
        office.Prerequisites.Add(new JobPrerequisite { RequiredJob = studBackend });
        office.Prerequisites.Add(new JobPrerequisite { RequiredJob = studDatabase });
        startup.Prerequisites.Add(new JobPrerequisite { RequiredJob = office });
        techCorp.Prerequisites.Add(new JobPrerequisite { RequiredJob = startup });

        // Save jobs, tasks and requirements in one go
        await db.SaveChangesAsync();

        logger.LogInformation("Database seeded with game content.");
    }
}