using LearnToDev.Api.DTOs;

namespace LearnToDev.Api.Validation;

public static class StudyQuestionValidator
{
    public static List<string> Validate(SaveStudyQuestionDto question)
    {
        var errors = new List<string>();
        var options = question.AnswerOptions ?? new List<AnswerOptionDto>();

        if (options.Count != 4)
        {
            errors.Add("The question must have exactly four answer options.");
        }

        if (options.Any(option =>
                string.IsNullOrWhiteSpace(option.Text)))
        {
            errors.Add("All answer options must have text.");
        }

        if (options.Count(option => option.IsCorrect) != 1)
        {
            errors.Add("Exactly one answer option must be correct.");
        }

        return errors;
    }
}