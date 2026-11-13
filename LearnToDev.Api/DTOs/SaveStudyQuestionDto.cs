using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Api.DTOs;

public class SaveStudyQuestionDto
{
    [Required]
    public int SchoolLevelId { get; set; }

    [Required, StringLength(1000)]
    public string Text { get; set; } = string.Empty;

    [Required, StringLength(2000)]
    public string Explanation { get; set; } = string.Empty;

    public List<AnswerOptionDto> AnswerOptions { get; set; } = new();
}