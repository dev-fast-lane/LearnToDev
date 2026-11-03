using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Mvc.Models;

public class AnswerOption
{
    public int Id { get; set; }

    public int StudyQuestionId { get; set; }
    public StudyQuestion StudyQuestion { get; set; } = null!;

    [Required, StringLength(1000)]
    public string Text { get; set; } = string.Empty;

    public bool IsCorrect { get; set; }
}