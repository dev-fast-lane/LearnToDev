using LearnToDev.Api.Models;
using LearnToDev.Api.Repositories.Interfaces;
using LearnToDev.Api.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace LearnToDev.Tests.Services;

public class JobEligibilityServiceTests
{
    private const int PlayerId = 1;
    private const int ItHelpId = 1;
    private const int StudAssFrontendId = 2;

    private readonly Mock<IJobRepository> _jobRepository = new();
    private readonly Mock<IPlayerRepository> _playerRepository = new();
    private readonly JobEligibilityService _service;

    // A small copy of the job ladder: IT Help -> Stud.ass Frontend (needs Frontend level 1 and IT Help)
    public JobEligibilityServiceTests()
    {
        var frontend = new School { Id = 1, Name = "Frontend" };
        var frontendLevel1 = new SchoolLevel { Id = 1, SchoolId = 1, School = frontend, Level = 1 };

        var itHelp = new Job { Id = ItHelpId, Name = "IT Help", Rank = 1 };
        var studAssFrontend = new Job { Id = StudAssFrontendId, Name = "Stud.ass Frontend", Rank = 2 };
        studAssFrontend.JobRequirements.Add(new JobRequirement { JobId = StudAssFrontendId, SchoolLevelId = 1, SchoolLevel = frontendLevel1 });
        studAssFrontend.Prerequisites.Add(new JobPrerequisite { JobId = StudAssFrontendId, RequiredJobId = ItHelpId, RequiredJob = itHelp });

        _jobRepository.Setup(r => r.GetByIdWithRequirementsAsync(ItHelpId)).ReturnsAsync(itHelp);
        _jobRepository.Setup(r => r.GetByIdWithRequirementsAsync(StudAssFrontendId)).ReturnsAsync(studAssFrontend);
        _jobRepository.Setup(r => r.GetAllWithRequirementsAsync()).ReturnsAsync(new List<Job> { itHelp, studAssFrontend });

        // NullLogger: a logger that throws away everything, because the tests do not check the log
        _service = new JobEligibilityService(_jobRepository.Object, _playerRepository.Object, NullLogger<JobEligibilityService>.Instance);
    }

    // Fake player with the given Frontend level (0 = not started) and maybe a completed day in IT Help
    private void SetUpPlayer(int frontendLevelPassed, bool hasWorkedItHelp)
    {
        var player = new Player { Id = PlayerId, Name = "Kari" };
        if (frontendLevelPassed > 0)
            player.SchoolProgresses.Add(new PlayerSchoolProgress { PlayerId = PlayerId, SchoolId = 1, LevelPassed = frontendLevelPassed });

        var completedJobIds = hasWorkedItHelp ? new List<int> { ItHelpId } : new List<int>();

        _playerRepository.Setup(r => r.GetByIdWithDetailsAsync(PlayerId)).ReturnsAsync(player);
        _playerRepository.Setup(r => r.GetCompletedJobIdsAsync(PlayerId)).ReturnsAsync(completedJobIds);
    }

    [Fact]
    public async Task CheckJobAsync_UnknownPlayer_ReturnsNull()
    {
        _playerRepository.Setup(r => r.GetByIdWithDetailsAsync(99)).ReturnsAsync((Player?)null);

        var result = await _service.CheckJobAsync(99, ItHelpId);

        Assert.Null(result);
        _jobRepository.Verify(r => r.GetByIdWithRequirementsAsync(It.IsAny<int>()), Times.Never); // Stops before asking for the job
    }

    [Fact]
    public async Task CheckJobAsync_UnknownJob_ReturnsNull()
    {
        SetUpPlayer(frontendLevelPassed: 0, hasWorkedItHelp: false);
        _jobRepository.Setup(r => r.GetByIdWithRequirementsAsync(99)).ReturnsAsync((Job?)null);

        var result = await _service.CheckJobAsync(PlayerId, 99);

        Assert.Null(result);
    }

    [Fact]
    public async Task CheckAllJobsAsync_NewPlayer_OnlyItHelpIsOpen()
    {
        SetUpPlayer(frontendLevelPassed: 0, hasWorkedItHelp: false);

        var result = await _service.CheckAllJobsAsync(PlayerId);

        Assert.Equal(new[] { "IT Help" }, result.Where(j => j.IsOpen).Select(j => j.JobName));
    }

    [Fact]
    public async Task CheckAllJobsAsync_UnknownPlayer_ReturnsEmptyList()
    {
        _playerRepository.Setup(r => r.GetByIdWithDetailsAsync(99)).ReturnsAsync((Player?)null);

        var result = await _service.CheckAllJobsAsync(99);

        Assert.Empty(result);
    }

    [Fact]
    public async Task CheckJobAsync_NoLevelAndNoItHelp_HasBothLockReasons()
    {
        SetUpPlayer(frontendLevelPassed: 0, hasWorkedItHelp: false);

        var result = await _service.CheckJobAsync(PlayerId, StudAssFrontendId);

        Assert.NotNull(result);
        Assert.False(result.IsOpen);
        Assert.Equal(new[] { "Mangler Frontend nivå 1", "Må ha jobbet som IT Help" }, result.LockReasons);
    }

    [Fact]
    public async Task CheckJobAsync_LevelPassedButNoItHelp_LockedOnlyByJob()
    {
        SetUpPlayer(frontendLevelPassed: 1, hasWorkedItHelp: false);

        var result = await _service.CheckJobAsync(PlayerId, StudAssFrontendId);

        Assert.NotNull(result);
        Assert.Equal(new[] { "Må ha jobbet som IT Help" }, result.LockReasons);
    }

    [Theory]
    [InlineData(1)] // Exactly the required level
    [InlineData(2)] // A higher level also counts (>=)
    public async Task CheckJobAsync_LevelPassedAndItHelpCompleted_IsOpen(int frontendLevelPassed)
    {
        SetUpPlayer(frontendLevelPassed, hasWorkedItHelp: true);

        var result = await _service.CheckJobAsync(PlayerId, StudAssFrontendId);

        Assert.NotNull(result);
        Assert.True(result.IsOpen);
        Assert.Empty(result.LockReasons);
    }
}
