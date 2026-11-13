using LearnToDev.Api.DTOs;
using LearnToDev.Api.Validation;

namespace LearnToDev.Tests.Validation;

public class StudyQuestionValidatorTests
{
    [Fact]
    public void Validate_WithFourOptionsAndOneCorrectOption_ReturnsNoErrors()
    {
        var question = CreateQuestion(
            new AnswerOptionDto { Text = "A", IsCorrect = true },
            new AnswerOptionDto { Text = "B" },
            new AnswerOptionDto { Text = "C" },
            new AnswerOptionDto { Text = "D" });

        var errors = StudyQuestionValidator.Validate(question);

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_WithWrongNumberOfOptions_ReturnsError()
    {
        var question = CreateQuestion(
            new AnswerOptionDto { Text = "A", IsCorrect = true },
            new AnswerOptionDto { Text = "B" },
            new AnswerOptionDto { Text = "C" });

        var errors = StudyQuestionValidator.Validate(question);

        Assert.Contains("nøyaktig fire svaralternativer", errors[0]);
    }

    [Fact]
    public void Validate_WithMissingOptionText_ReturnsError()
    {
        var question = CreateQuestion(
            new AnswerOptionDto { Text = "A", IsCorrect = true },
            new AnswerOptionDto { Text = " " },
            new AnswerOptionDto { Text = "C" },
            new AnswerOptionDto { Text = "D" });

        var errors = StudyQuestionValidator.Validate(question);

        Assert.Contains("Alle svaralternativene må ha tekst.", errors);
    }

    [Fact]
    public void Validate_WithZeroOrMultipleCorrectOptions_ReturnsError()
    {
        var question = CreateQuestion(
            new AnswerOptionDto { Text = "A", IsCorrect = true },
            new AnswerOptionDto { Text = "B", IsCorrect = true },
            new AnswerOptionDto { Text = "C" },
            new AnswerOptionDto { Text = "D" });

        var errors = StudyQuestionValidator.Validate(question);

        Assert.Contains("Nøyaktig ett svaralternativ må være riktig.", errors);
    }

    private static SaveStudyQuestionDto CreateQuestion(params AnswerOptionDto[] options)
    {
        return new SaveStudyQuestionDto
        {
            Text = "Et spørsmål",
            Explanation = "En forklaring",
            AnswerOptions = options.ToList()
        };
    }
}
