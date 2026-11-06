namespace LearnToDev.Api.Services.Interfaces;

public record ExamAccessResult(bool CanTake, string? Reason = null)
{
    public static ExamAccessResult Accept() => new(true);

    public static ExamAccessResult Reject(string reason) => new(false, reason);
}

public interface IExamService
{

    Task<ExamAccessResult> CanTakeExamAsync(int playerId, int schoolId, int level);
}