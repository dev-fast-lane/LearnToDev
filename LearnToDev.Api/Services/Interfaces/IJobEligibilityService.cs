namespace LearnToDev.Api.Services.Interfaces;

// Decides which jobs a player can take, and why the others are locked
public interface IJobEligibilityService
{
    // Checks one job for one player. Returns null if the player or the job does not exist
    Task<JobEligibility?> CheckJobAsync(int playerId, int jobId);

    // Checks every job for one player. Returns an empty list if the player does not exist
    Task<List<JobEligibility>> CheckAllJobsAsync(int playerId);
}

