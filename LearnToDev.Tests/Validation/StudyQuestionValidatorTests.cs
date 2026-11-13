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

        Assert.Contains("exactly four answer options", errors[0]);
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

        Assert.Contains("All answer options must have text.", errors);
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

        Assert.Contains("Exactly one answer option must be correct.", errors);
    }

    private static SaveStudyQuestionDto CreateQuestion(params AnswerOptionDto[] options)
    {
        return new SaveStudyQuestionDto
        {
            Text = "A question",
            Explanation = "An explanation",
            AnswerOptions = options.ToList()
        };
    }
}
