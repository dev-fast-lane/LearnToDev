using LearnToDev.Api.Models;

namespace LearnToDev.Api.Repositories.Interfaces;

public interface IStudyQuestionRepository
{
    Task<List<StudyQuestion>> GetAllAsync();
    Task<StudyQuestion?> GetByIdAsync(int id);
    Task<StudyQuestion> AddAsync(StudyQuestion studyQuestion);
    Task<bool> UpdateAsync(StudyQuestion studyQuestion);
    Task<bool> DeleteAsync(int id);
    Task<StudyQuestion?> GetByIdWithAnswerOptionsAsync(int id);
    Task<List<StudyQuestion>> GetBySchoolLevelWithAnswerOptionsAsync(int schoolLevelId);
}
