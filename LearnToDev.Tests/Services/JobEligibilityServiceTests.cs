using LearnToDev.Api.Models;
using LearnToDev.Api.Repositories.Interfaces;
using LearnToDev.Api.Services;
using LearnToDev.Api.Services.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace LearnToDev.Tests.Services;

public class JobEligibilityServiceTests
{
    private const int PlayerId = 1;
    private const int ItHelpId = 1;
    private const int StudAssFrontendId = 2;
    private const int CombinedJobId = 3;

    // Only used by the Office test. Any free numbers work, they just have to match each other
    private const int StudAssBackendId = 5;
    private const int StudAssDatabaseId = 6;
    private const int OfficeId = 7;
    private const int FullstackSchoolId = 4;
    private const int FullstackLevel1Id = 10;

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

        // Test-only job with two of each, to check that every requirement and every prerequisite is needed
        var backend = new School { Id = 2, Name = "Backend" };
        var backendLevel1 = new SchoolLevel { Id = 2, SchoolId = 2, School = backend, Level = 1 };
        var combinedJob = new Job { Id = CombinedJobId, Name = "Combined", Rank = 3 };
        combinedJob.JobRequirements.Add(new JobRequirement { JobId = CombinedJobId, SchoolLevelId = 1, SchoolLevel = frontendLevel1 });
        combinedJob.JobRequirements.Add(new JobRequirement { JobId = CombinedJobId, SchoolLevelId = 2, SchoolLevel = backendLevel1 });
        combinedJob.Prerequisites.Add(new JobPrerequisite { JobId = CombinedJobId, RequiredJobId = ItHelpId, RequiredJob = itHelp });
        combinedJob.Prerequisites.Add(new JobPrerequisite { JobId = CombinedJobId, RequiredJobId = StudAssFrontendId, RequiredJob = studAssFrontend });

        _jobRepository.Setup(r => r.GetByIdWithRequirementsAsync(ItHelpId)).ReturnsAsync(itHelp);
        _jobRepository.Setup(r => r.GetByIdWithRequirementsAsync(StudAssFrontendId)).ReturnsAsync(studAssFrontend);
        _jobRepository.Setup(r => r.GetByIdWithRequirementsAsync(CombinedJobId)).ReturnsAsync(combinedJob);
        _jobRepository.Setup(r => r.GetAllWithRequirementsAsync()).ReturnsAsync(new List<Job> { itHelp, studAssFrontend });

        // NullLogger: a logger that throws away everything, because the tests do not check the log
        _service = new JobEligibilityService(_jobRepository.Object, _playerRepository.Object, NullLogger<JobEligibilityService>.Instance);
    }

    // Fake player with the given school levels (0 = not started) and maybe completed days in IT Help and Stud.ass Frontend
    private void SetUpPlayer(int frontendLevelPassed, bool hasWorkedItHelp, int backendLevelPassed = 0, bool hasWorkedStudAssFrontend = false)
    {
        var player = new Player { Id = PlayerId, Name = "Kari" };
        if (frontendLevelPassed > 0)
            player.SchoolProgresses.Add(new PlayerSchoolProgress { PlayerId = PlayerId, SchoolId = 1, LevelPassed = frontendLevelPassed });
        if (backendLevelPassed > 0)
            player.SchoolProgresses.Add(new PlayerSchoolProgress { PlayerId = PlayerId, SchoolId = 2, LevelPassed = backendLevelPassed });

        var completedJobIds = new HashSet<int>();
        if (hasWorkedItHelp)
            completedJobIds.Add(ItHelpId);
        if (hasWorkedStudAssFrontend)
            completedJobIds.Add(StudAssFrontendId);

        _playerRepository.Setup(r => r.GetByIdWithSchoolProgressAsync(PlayerId)).ReturnsAsync(player);
        _playerRepository.Setup(r => r.GetCompletedJobIdsAsync(PlayerId)).ReturnsAsync(completedJobIds);
    }

    [Fact]
    public async Task CheckJobAsync_UnknownPlayer_ReturnsNull()
    {
        _playerRepository.Setup(r => r.GetByIdWithSchoolProgressAsync(99)).ReturnsAsync((Player?)null);

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

        Assert.NotNull(result);
        Assert.Equal(new[] { "IT Help" }, result.Where(j => j.IsOpen).Select(j => j.JobName));
    }

    [Fact]
    public async Task CheckAllJobsAsync_UnknownPlayer_ReturnsNull()
    {
        _playerRepository.Setup(r => r.GetByIdWithSchoolProgressAsync(99)).ReturnsAsync((Player?)null);

        var result = await _service.CheckAllJobsAsync(99);

        Assert.Null(result); // null, not an empty list, so the controller can answer 404
    }

    [Fact]
    public async Task CheckJobAsync_TwoRequirementsOnlyOnePassed_LockedByTheMissingOne()
    {
        SetUpPlayer(frontendLevelPassed: 1, hasWorkedItHelp: true, backendLevelPassed: 0, hasWorkedStudAssFrontend: true);

        var result = await _service.CheckJobAsync(PlayerId, CombinedJobId);

        Assert.NotNull(result);
        Assert.False(result.IsOpen); // "All": one passed level is not enough
        Assert.Equal(new[] { "Mangler Backend nivå 1" }, result.LockReasons);
    }

    [Fact]
    public async Task CheckJobAsync_TwoOfEachMissing_ListsEveryMissingItem()
    {
        SetUpPlayer(frontendLevelPassed: 0, hasWorkedItHelp: false);

        var result = await _service.CheckJobAsync(PlayerId, CombinedJobId);

        Assert.NotNull(result);
        Assert.Equal(new[] { "Mangler Frontend nivå 1 og Backend nivå 1", "Må ha jobbet som IT Help og Stud.ass Frontend" }, result.LockReasons);
    }

    [Fact]
    public async Task CheckJobAsync_TwoOfEachCompleted_IsOpen()
    {
        SetUpPlayer(frontendLevelPassed: 1, hasWorkedItHelp: true, backendLevelPassed: 1, hasWorkedStudAssFrontend: true);

        var result = await _service.CheckJobAsync(PlayerId, CombinedJobId);

        Assert.NotNull(result);
        Assert.True(result.IsOpen);
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

    [Fact]
    public async Task CheckJobAsync_OfficeWithAllStudAssButNoFullstack_LockedByFullstack()
    {
        // Arrange: Office needs Fullstack level 1 and a work day in all three Stud.ass jobs
        var fullstack = new School { Id = FullstackSchoolId, Name = "Fullstack" };
        var fullstackLevel1 = new SchoolLevel { Id = FullstackLevel1Id, SchoolId = FullstackSchoolId, School = fullstack, Level = 1 };

        var studAssFrontend = new Job { Id = StudAssFrontendId, Name = "Stud.ass Frontend" };
        var studAssBackend = new Job { Id = StudAssBackendId, Name = "Stud.ass Backend" };
        var studAssDatabase = new Job { Id = StudAssDatabaseId, Name = "Stud.ass Database" };

        var office = new Job { Id = OfficeId, Name = "Office" };
        office.JobRequirements.Add(new JobRequirement { JobId = OfficeId, SchoolLevelId = FullstackLevel1Id, SchoolLevel = fullstackLevel1 });
        office.Prerequisites.Add(new JobPrerequisite { JobId = OfficeId, RequiredJobId = StudAssFrontendId, RequiredJob = studAssFrontend });
        office.Prerequisites.Add(new JobPrerequisite { JobId = OfficeId, RequiredJobId = StudAssBackendId, RequiredJob = studAssBackend });
        office.Prerequisites.Add(new JobPrerequisite { JobId = OfficeId, RequiredJobId = StudAssDatabaseId, RequiredJob = studAssDatabase });

        // The player has worked in every Stud.ass job and passed Frontend level 2, but not Fullstack
        var player = new Player { Id = PlayerId, Name = "Kari" };
        player.SchoolProgresses.Add(new PlayerSchoolProgress { PlayerId = PlayerId, SchoolId = 1, LevelPassed = 2 });
        var completedJobIds = new HashSet<int> { ItHelpId, StudAssFrontendId, StudAssBackendId, StudAssDatabaseId };

        _jobRepository.Setup(r => r.GetByIdWithRequirementsAsync(OfficeId)).ReturnsAsync(office);
        _playerRepository.Setup(r => r.GetByIdWithSchoolProgressAsync(PlayerId)).ReturnsAsync(player);
        _playerRepository.Setup(r => r.GetCompletedJobIdsAsync(PlayerId)).ReturnsAsync(completedJobIds);

        // Act
        var result = await _service.CheckJobAsync(PlayerId, OfficeId);

        // Assert: locked by the school rule only, so the Stud.ass jobs were accepted
        Assert.NotNull(result);
        Assert.False(result.IsOpen);
        Assert.Equal(new[] { "Mangler Fullstack nivå 1" }, result.LockReasons);
    }
}
