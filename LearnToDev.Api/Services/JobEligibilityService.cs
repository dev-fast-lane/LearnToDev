using LearnToDev.Api.Models;
using LearnToDev.Api.Repositories.Interfaces;

namespace LearnToDev.Api.Services;

// Checks school and job requirements to decide which jobs are open for a player
public class JobEligibilityService : IJobEligibilityService
{
    private readonly IJobRepository _jobRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ILogger<JobEligibilityService> _logger;

    // DI: Program.cs gives us the repositories and the logger
    public JobEligibilityService(IJobRepository jobRepository, IPlayerRepository playerRepository, ILogger<JobEligibilityService> logger)
    {
        _jobRepository = jobRepository;
        _playerRepository = playerRepository;
        _logger = logger;
    }

    public Task<JobEligibility?> CheckJobAsync(int playerId, int jobId)
    {
        throw new NotImplementedException(); // Implemented in step 6
    }

    public Task<List<JobEligibility>> CheckAllJobsAsync(int playerId)
    {
        throw new NotImplementedException(); // Implemented in step 6
    }

    // Returns a lock reason if the player has not passed any of the job's school requirements, otherwise null
    private static string? CheckSchoolRequirements(Job job, Player player)
    {
        // Jobs without school requirements (IT Help) pass this rule
        if (job.JobRequirements.Count == 0)
            return null;

        // "At least one": passing one of the required levels is enough
        bool hasPassedOne = job.JobRequirements.Any(requirement =>
            player.SchoolProgresses.Any(progress =>
                progress.SchoolId == requirement.SchoolLevel.SchoolId &&
                progress.LevelPassed >= requirement.SchoolLevel.Level));

        if (hasPassedOne)
            return null;

        // e.g. "Mangler Frontend nivå 1", or "Mangler Frontend nivå 1 eller Backend nivå 1"
        var missing = job.JobRequirements
            .Select(requirement => $"{requirement.SchoolLevel.School.Name} nivå {requirement.SchoolLevel.Level}");

        return $"Mangler {string.Join(" eller ", missing)}";
    }

    // Returns a lock reason if the player has not completed a work day in every prerequisite job, otherwise null
    private static string? CheckPrerequisiteJobs(Job job, List<int> completedJobIds)
    {
        // "All": every prerequisite job must be completed, so collect the ones that are missing
        var missingJobs = job.Prerequisites
            .Where(prerequisite => !completedJobIds.Contains(prerequisite.RequiredJobId))
            .Select(prerequisite => prerequisite.RequiredJob.Name)
            .ToList();

        if (missingJobs.Count == 0)
            return null;

        // e.g. "Må ha jobbet som Stud.ass Backend", or "Må ha jobbet som Stud.ass Frontend, Stud.ass Backend"
        return $"Må ha jobbet som {string.Join(", ", missingJobs)}";
    }
}