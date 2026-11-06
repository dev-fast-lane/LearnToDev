using LearnToDev.Api.Data;
using LearnToDev.Api.Models;
using LearnToDev.Api.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace LearnToDev.Tests.Repositories;

//Kjører mot en ekte SQLite-database i minnet, så Include, nøkler og fremmednøkler også blir testet
public class PlayerRepositoryTests : IDisposable
{
    private const string UserId = "user-1";
    private const string OtherUserId = "user-2";

    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _context;
    private readonly PlayerRepository _repository;

    public PlayerRepositoryTests()
    {
        //Databasen i minnet lever bare så lenge tilkoblingen er åpen
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();

        _context.Users.AddRange(
            new IdentityUser { Id = UserId, UserName = "en" },
            new IdentityUser { Id = OtherUserId, UserName = "to" });
        _context.Housings.Add(new Housing { Id = 1, Name = "Telt", Order = 1, HoursPerDay = 12 });
        _context.Jobs.AddRange(
            new Job { Id = 1, Name = "IT Help", Rank = 1, Wage = 20, TasksPerDay = 5 },
            new Job { Id = 2, Name = "Stud.ass", Rank = 2, Wage = 75, TasksPerDay = 5 });
        _context.Schools.Add(new School { Id = 1, Name = "Backend" });
        _context.SaveChanges();

        _repository = new PlayerRepository(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    private static Player NewPlayer(string userId = UserId, string name = "Kari")
    {
        return new Player { UserId = userId, Name = name, Money = 50, HoursLeftInDay = 12, HousingId = 1 };
    }

    [Fact]
    public async Task AddAsync_NewPlayer_SetsId()
    {
        var player = await _repository.AddAsync(NewPlayer());

        Assert.True(player.Id > 0);
    }

    [Fact]
    public async Task GetByIdWithDetailsAsync_ExistingPlayer_IncludesHousingJobAndProgress()
    {
        var player = NewPlayer();
        player.CurrentJobId = 1;
        await _repository.AddAsync(player);
        await _repository.SaveSchoolProgressAsync(player.Id, 1, 1);
        _context.ChangeTracker.Clear(); // Glem objektene som er lastet, så bare Include kan fylle dem inn

        var result = await _repository.GetByIdWithDetailsAsync(player.Id);

        Assert.NotNull(result);
        Assert.Equal("Telt", result.Housing.Name);
        Assert.Equal("IT Help", result.CurrentJob?.Name);
        Assert.Single(result.SchoolProgresses);
    }

    [Fact]
    public async Task GetByIdWithDetailsAsync_UnknownId_ReturnsNull()
    {
        var result = await _repository.GetByIdWithDetailsAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdWithSchoolProgressAsync_ExistingPlayer_IncludesProgress()
    {
        var player = await _repository.AddAsync(NewPlayer());
        await _repository.SaveSchoolProgressAsync(player.Id, 1, 1);
        _context.ChangeTracker.Clear(); // Glem objektene som er lastet, så bare Include kan fylle dem inn

        var result = await _repository.GetByIdWithSchoolProgressAsync(player.Id);

        Assert.NotNull(result);
        Assert.Single(result.SchoolProgresses);
    }

    [Fact]
    public async Task GetByIdWithSchoolProgressAsync_UnknownId_ReturnsNull()
    {
        var result = await _repository.GetByIdWithSchoolProgressAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUserIdAsync_TwoAccounts_ReturnsOnlyPlayersOnThatAccount()
    {
        await _repository.AddAsync(NewPlayer(UserId, "Kari"));
        await _repository.AddAsync(NewPlayer(UserId, "Ola"));
        await _repository.AddAsync(NewPlayer(OtherUserId, "Per"));

        var result = await _repository.GetByUserIdAsync(UserId);

        Assert.Equal(new[] { "Kari", "Ola" }, result.Select(p => p.Name));
    }

    [Fact]
    public async Task UpdateAsync_ExistingPlayer_SavesChanges()
    {
        var player = await _repository.AddAsync(NewPlayer());
        player.Money = 100;

        var updated = await _repository.UpdateAsync(player);
        _context.ChangeTracker.Clear();

        Assert.True(updated);
        Assert.Equal(100, (await _context.Players.FindAsync(player.Id))?.Money);
    }

    [Fact]
    public async Task UpdateAsync_UnknownPlayer_ReturnsFalse()
    {
        var player = NewPlayer();
        player.Id = 999;

        var updated = await _repository.UpdateAsync(player);

        Assert.False(updated);
    }

    [Fact]
    public async Task GetCompletedJobDaysAsync_MixedDays_ReturnsOnlyCompletedOnThatDayWithJob()
    {
        var player = await _repository.AddAsync(NewPlayer());
        var other = await _repository.AddAsync(NewPlayer(OtherUserId, "Per"));
        await _repository.AddJobDayAsync(new JobDay { PlayerId = player.Id, JobId = 1, Day = 1, Completed = true });
        await _repository.AddJobDayAsync(new JobDay { PlayerId = player.Id, JobId = 1, Day = 2, Completed = true });
        await _repository.AddJobDayAsync(new JobDay { PlayerId = player.Id, JobId = 2, Day = 1, Completed = false });
        await _repository.AddJobDayAsync(new JobDay { PlayerId = other.Id, JobId = 1, Day = 1, Completed = true });
        _context.ChangeTracker.Clear();

        var result = await _repository.GetCompletedJobDaysAsync(player.Id, 1);

        var jobDay = Assert.Single(result);
        Assert.Equal(20, jobDay.Job.Wage);
    }

    [Fact]
    public async Task GetCompletedJobIdsAsync_SeveralDaysInSameJob_ReturnsEachJobOnce()
    {
        var player = await _repository.AddAsync(NewPlayer());
        await _repository.AddJobDayAsync(new JobDay { PlayerId = player.Id, JobId = 1, Day = 1, Completed = true });
        await _repository.AddJobDayAsync(new JobDay { PlayerId = player.Id, JobId = 1, Day = 2, Completed = true });
        await _repository.AddJobDayAsync(new JobDay { PlayerId = player.Id, JobId = 2, Day = 3, Completed = false });

        var result = await _repository.GetCompletedJobIdsAsync(player.Id);

        Assert.Equal(new[] { 1 }, result);
    }

    [Fact]
    public async Task SaveSchoolProgressAsync_CalledTwice_UpdatesTheSameRow()
    {
        var player = await _repository.AddAsync(NewPlayer());

        await _repository.SaveSchoolProgressAsync(player.Id, 1, 1);
        await _repository.SaveSchoolProgressAsync(player.Id, 1, 2);
        _context.ChangeTracker.Clear();

        var progress = await _context.PlayerSchoolProgresses.SingleAsync(p => p.PlayerId == player.Id);
        Assert.Equal(2, progress.LevelPassed);
    }
}
