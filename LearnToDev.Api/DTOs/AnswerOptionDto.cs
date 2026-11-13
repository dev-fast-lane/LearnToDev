using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Api.DTOs;

public class AnswerOptionDto
{
    [Required, StringLength(1000)]
    public string Text {get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}