using LearnToDev.Api.Models;
using LearnToDev.Api.Repositories.Interfaces;
using LearnToDev.Api.Services.Interfaces;

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

    public async Task<JobEligibility?> CheckJobAsync(int playerId, int jobId)
    {
        var player = await _playerRepository.GetByIdWithSchoolProgressAsync(playerId);
        if (player == null)
        {
            _logger.LogWarning("Eligibility check failed: player {PlayerId} not found", playerId);
            return null;
        }

        var job = await _jobRepository.GetByIdWithRequirementsAsync(jobId);
        if (job == null)
        {
            _logger.LogWarning("Eligibility check failed: job {JobId} not found", jobId);
            return null;
        }

        var completedJobIds = await _playerRepository.GetCompletedJobIdsAsync(playerId);
        var result = Evaluate(job, player, completedJobIds);

        _logger.LogDebug("Player {PlayerId} checked job {JobId}: open = {IsOpen}", playerId, jobId, result.IsOpen);
        return result;
    }

    public async Task<List<JobEligibility>?> CheckAllJobsAsync(int playerId)
    {
        var player = await _playerRepository.GetByIdWithSchoolProgressAsync(playerId);
        if (player == null)
        {
            _logger.LogWarning("Eligibility check failed: player {PlayerId} not found", playerId);
            return null;
        }

        var jobs = await _jobRepository.GetAllWithRequirementsAsync();
        var completedJobIds = await _playerRepository.GetCompletedJobIdsAsync(playerId);

        var results = jobs.Select(job => Evaluate(job, player, completedJobIds)).ToList();

        _logger.LogDebug("Player {PlayerId} has {OpenCount} of {JobCount} jobs open",
            playerId, results.Count(r => r.IsOpen), results.Count);
        return results;
    }

    // Runs every rule for one job and collects the lock reasons. The job is open when there are none
    private static JobEligibility Evaluate(Job job, Player player, HashSet<int> completedJobIds)
    {
        var lockReasons = new List<string?>
        {
            CheckSchoolRequirements(job, player),
            CheckPrerequisiteJobs(job, completedJobIds),
            // #40: add the "fired from this job" rule here
        }
        .OfType<string>() // Removes the nulls, so only real lock reasons are left
        .ToList();

        return new JobEligibility(job.Id, job.Name, lockReasons.Count == 0, lockReasons);
    }

    // Returns a lock reason if the player has not passed every school requirement of the job, otherwise null
    private static string? CheckSchoolRequirements(Job job, Player player)
    {
        // "All", like the prerequisite jobs: collect the required levels the player has not passed.
        // Jobs without school requirements (IT Help) get an empty list and pass this rule
        // Only the highest required level per school counts, since passing level 2 means level 1 is passed too
        var missing = job.JobRequirements
            .GroupBy(requirement => requirement.SchoolLevel.SchoolId)
            .Select(group => group.MaxBy(requirement => requirement.SchoolLevel.Level)!)
            .Where(requirement => player.GetLevelPassed(requirement.SchoolLevel.SchoolId) < requirement.SchoolLevel.Level)
            .Select(requirement => $"{requirement.SchoolLevel.School.Name} nivå {requirement.SchoolLevel.Level}")
            .ToList();

        if (missing.Count == 0)
            return null;

        // e.g. "Mangler Frontend nivå 1", or "Mangler Frontend nivå 1 og Backend nivå 1"
        return $"Mangler {JoinWithAnd(missing)}";
    }

    // Returns a lock reason if the player has not completed a work day in every prerequisite job, otherwise null
    private static string? CheckPrerequisiteJobs(Job job, HashSet<int> completedJobIds)
    {
        // "All": every prerequisite job must be completed, so collect the ones that are missing
        var missingJobs = job.Prerequisites
            .Where(prerequisite => !completedJobIds.Contains(prerequisite.RequiredJobId))
            .Select(prerequisite => prerequisite.RequiredJob.Name)
            .ToList();

        if (missingJobs.Count == 0)
            return null;

        // e.g. "Må ha jobbet som IT Help", or "Må ha jobbet som Stud.ass Frontend, Stud.ass Backend og Stud.ass Database"
        return $"Må ha jobbet som {JoinWithAnd(missingJobs)}";
    }

    // Joins a list the Norwegian way: "A", "A og B", "A, B og C"
    private static string JoinWithAnd(List<string> items)
    {
        if (items.Count == 1)
            return items[0];

        return $"{string.Join(", ", items.Take(items.Count - 1))} og {items[^1]}";
    }
}