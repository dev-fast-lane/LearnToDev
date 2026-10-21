using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Mvc.Models;

public class StudyQuestion
{
    public int Id { get; set; }

    //The school level the question belongs to (SchoolLevelId is stored in the database, SchoolLevel gives the object)
    public int SchoolLevelId { get; set; }
    public SchoolLevel SchoolLevel { get; set; } = null!;

    [Required, StringLength(1000)]
    public string Text { get; set; } = string.Empty;

    [Required, StringLength(2000)]
    public string Explanation { get; set; } = string.Empty;

    //The answer options for the question (one question has several options, exactly one is correct)
    public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
}