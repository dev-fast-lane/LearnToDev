using LearnToDev.Api.Data;
using LearnToDev.Api.Models;
using LearnToDev.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearnToDev.Api.Repositories;

public class StudyQuestionRepository : IStudyQuestionRepository
{

    private readonly ApplicationDbContext _context;
    public StudyQuestionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudyQuestion> AddAsync(StudyQuestion studyQuestion)
    {
        _context.StudyQuestions.Add(studyQuestion);
        await _context.SaveChangesAsync();
        return studyQuestion;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var toBeDeleted = await _context.StudyQuestions.FindAsync(id);

        if (toBeDeleted == null) return false;

        _context.StudyQuestions.Remove(toBeDeleted);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<StudyQuestion>> GetAllAsync()
    {
        return await _context.StudyQuestions
            .Include(q => q.SchoolLevel)
            .ThenInclude(level => level.School)
            .Include(q => q.AnswerOptions)
            .ToListAsync();
    }

    public async Task<StudyQuestion?> GetByIdAsync(int id)
    {
        return await _context.StudyQuestions.FindAsync(id);
    }

    public async Task<StudyQuestion?> GetByIdWithAnswerOptionsAsync(int id)
    {
        return await _context.StudyQuestions
            .Include(q => q.SchoolLevel)
            .ThenInclude(level => level.School)
            .Include(q => q.AnswerOptions)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    //All questions in one school level with their answer options, used by the exam (#31)
    public async Task<List<StudyQuestion>> GetBySchoolLevelWithAnswerOptionsAsync(int schoolLevelId)
    {
        return await _context.StudyQuestions
            .Include(q => q.SchoolLevel)
            .ThenInclude(level => level.School)
            .Include(q => q.AnswerOptions)
            .Where(q => q.SchoolLevelId == schoolLevelId)
            .ToListAsync();
    }

    public async Task<bool> UpdateAsync(StudyQuestion studyQuestion)
    {
        var oldStudyQuestion = await _context.StudyQuestions
            .Include(q => q.AnswerOptions)
            .FirstOrDefaultAsync(q => q.Id == studyQuestion.Id);

        if (oldStudyQuestion == null) return false;

        _context.Entry(oldStudyQuestion).CurrentValues.SetValues(studyQuestion);
        _context.AnswerOptions.RemoveRange(oldStudyQuestion.AnswerOptions);
        oldStudyQuestion.AnswerOptions = studyQuestion.AnswerOptions;
        await _context.SaveChangesAsync();
        return true;
    }
}
