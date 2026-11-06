namespace LearnToDev.Api.Services;

// The answer for one job: can the player take it, and if not, why
public record JobEligibility(int JobId, string JobName, bool IsOpen, List<string> LockReasons);