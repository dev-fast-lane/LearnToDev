using LearnToDev.Api.DTOs;
using LearnToDev.Api.Models;
using LearnToDev.Api.Repositories.Interfaces;
using LearnToDev.Api.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnToDev.Api.Controllers;

[ApiController]
[Route("api/questions")]
[Authorize(Roles = Roles.Admin)]
public class QuestionsController : ControllerBase
{
    private readonly IStudyQuestionRepository _questionRepository;
    private readonly ISchoolLevelRepository _schoolLevelRepository;
    private readonly ILogger<QuestionsController> _logger;

    public QuestionsController(
        IStudyQuestionRepository questionRepository,
        ISchoolLevelRepository schoolLevelRepository,
        ILogger<QuestionsController> logger)
    {
        _questionRepository = questionRepository;
        _schoolLevelRepository = schoolLevelRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<StudyQuestionDto>>> GetAll()
    {
        var questions = await _questionRepository.GetAllAsync();
        return Ok(questions.Select(ToDto).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudyQuestionDto>> GetById(int id)
    {
        var question = await _questionRepository.GetByIdWithAnswerOptionsAsync(id);

        if (question == null)
        {
            _logger.LogWarning("Study question {QuestionId} was not found", id);
            return NotFound();
        }

        return Ok(ToDto(question));
    }

    [HttpPost]
    public async Task<ActionResult<StudyQuestionDto>> Create(SaveStudyQuestionDto dto)
    {
        if (!TryValidateQuestion(dto))
            return ValidationProblem(ModelState);

        var schoolLevel = await _schoolLevelRepository.GetByIdAsync(dto.SchoolLevelId);
        if (schoolLevel == null)
        {
            ModelState.AddModelError(nameof(dto.SchoolLevelId), "Skolenivået finnes ikke.");
            return ValidationProblem(ModelState);
        }

        var question = ToEntity(dto);
        question.SchoolLevel = schoolLevel;
        var created = await _questionRepository.AddAsync(question);
        var result = await _questionRepository.GetByIdWithAnswerOptionsAsync(created.Id);

        _logger.LogInformation("Created study question {QuestionId}", created.Id);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToDto(result!));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<StudyQuestionDto>> Update(int id, SaveStudyQuestionDto dto)
    {
        if (!TryValidateQuestion(dto))
            return ValidationProblem(ModelState);

        var schoolLevel = await _schoolLevelRepository.GetByIdAsync(dto.SchoolLevelId);
        if (schoolLevel == null)
        {
            ModelState.AddModelError(nameof(dto.SchoolLevelId), "Skolenivået finnes ikke.");
            return ValidationProblem(ModelState);
        }

        var question = ToEntity(dto);
        question.Id = id;
        var updated = await _questionRepository.UpdateAsync(question);

        if (!updated)
        {
            _logger.LogWarning("Study question {QuestionId} was not found for update", id);
            return NotFound();
        }

        var result = await _questionRepository.GetByIdWithAnswerOptionsAsync(id);
        _logger.LogInformation("Updated study question {QuestionId}", id);
        return Ok(ToDto(result!));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _questionRepository.DeleteAsync(id);

        if (!deleted)
        {
            _logger.LogWarning("Study question {QuestionId} was not found for deletion", id);
            return NotFound();
        }

        _logger.LogInformation("Deleted study question {QuestionId}", id);
        return NoContent();
    }

    private bool TryValidateQuestion(SaveStudyQuestionDto dto)
    {
        foreach (var error in StudyQuestionValidator.Validate(dto))
            ModelState.AddModelError(nameof(dto.AnswerOptions), error);

        return ModelState.IsValid;
    }

    private static StudyQuestion ToEntity(SaveStudyQuestionDto dto)
    {
        return new StudyQuestion
        {
            SchoolLevelId = dto.SchoolLevelId,
            Text = dto.Text,
            Explanation = dto.Explanation,
            AnswerOptions = dto.AnswerOptions
                .Select(option => new AnswerOption
                {
                    Text = option.Text,
                    IsCorrect = option.IsCorrect
                })
                .ToList()
        };
    }

    private static StudyQuestionDto ToDto(StudyQuestion question)
    {
        return new StudyQuestionDto
        {
            Id = question.Id,
            SchoolLevelId = question.SchoolLevelId,
            SchoolName = question.SchoolLevel?.School?.Name ?? string.Empty,
            Level = question.SchoolLevel?.Level ?? 0,
            Text = question.Text,
            Explanation = question.Explanation,
            AnswerOptions = question.AnswerOptions
                .Select(option => new AnswerOptionDto
                {
                    Text = option.Text,
                    IsCorrect = option.IsCorrect
                })
                .ToList()
        };
    }
}
