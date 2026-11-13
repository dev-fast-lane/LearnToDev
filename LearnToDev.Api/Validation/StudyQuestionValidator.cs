using LearnToDev.Api.DTOs;

namespace LearnToDev.Api.Validation;

public static class StudyQuestionValidator
{
    public static List<string> Validate(SaveStudyQuestionDto question)
    {
        var errors = new List<string>();

        if (question.AnswerOptions.Count != 4)
        {
            errors.Add("The question must have exactly four answer options.");
        }

        if (question.AnswerOptions.Any(option =>
                string.IsNullOrWhiteSpace(option.Text)))
        {
            errors.Add("All answer options must have text.");
        }

        if (question.AnswerOptions.Count(option => option.IsCorrect) != 1)
        {
            errors.Add("Exactly one answer option must be correct.");
        }

        return errors;
    }
}