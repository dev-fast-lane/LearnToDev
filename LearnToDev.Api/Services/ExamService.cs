using LearnToDev.Api.Models;
using LearnToDev.Api.Repositories.Interfaces;
using LearnToDev.Api.Services.Interfaces;

namespace LearnToDev.Api.Services;

public class ExamService : IExamService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ISchoolRepository _schoolRepository;

    public ExamService(IPlayerRepository playerRepository, ISchoolRepository schoolRepository)
    {
        _playerRepository = playerRepository;
        _schoolRepository = schoolRepository;
    }

    public async Task<ExamAccessResult> CanTakeExamAsync(int playerId, int schoolId, int level)
    {
        var player = await _playerRepository.GetByIdWithSchoolProgressAsync(playerId);
        if (player == null)
            return ExamAccessResult.Reject("Fant ikke spilleren.");

        var school = await _schoolRepository.GetByIdAsync(schoolId);
        if (school == null)
            return ExamAccessResult.Reject("Fant ikke skolen.");

        if (level < 1 || level > GameRules.MaxSchoolLevel)
            return ExamAccessResult.Reject($"Nivået må være mellom 1 og {GameRules.MaxSchoolLevel}.");

        var passedInSchool = player.GetLevelPassed(school.Id);
        if (passedInSchool >= level)
            return ExamAccessResult.Reject($"Du har allerede bestått {school.Name} nivå {level}.");


        if (passedInSchool < level - 1)
            return ExamAccessResult.Reject($"Du må bestå {school.Name} nivå {level - 1} først.");


        if (school.IsExamOnly)
        {
            var regularSchools = (await _schoolRepository.GetAllAsync())
                .Where(s => !s.IsExamOnly)
                .OrderBy(s => s.Name);

            var missing = regularSchools.FirstOrDefault(s => player.GetLevelPassed(s.Id) < level);
            if (missing != null)
                return ExamAccessResult.Reject($"Du må bestå {missing.Name} nivå {level} først.");
        }

        return ExamAccessResult.Accept();
    }
}
