using LearnToDev.Api.DTOs;

namespace LearnToDev.Api.Validation;

public static class StudyQuestionValidator
{
    public static List<string> Validate(SaveStudyQuestionDto question)
    {
        var errors = new List<string>();

        if (question.AnswerOptions.Count != 4)
        {
            errors.Add("Spørsmålet må ha nøyaktig fire svaralternativer.");
        }

        if (question.AnswerOptions.Any(option =>
                string.IsNullOrWhiteSpace(option.Text)))
        {
            errors.Add("Alle svaralternativene må ha tekst.");
        }

        if (question.AnswerOptions.Count(option => option.IsCorrect) != 1)
        {
            errors.Add("Nøyaktig ett svaralternativ må være riktig.");
        }

        return errors;
    }
}