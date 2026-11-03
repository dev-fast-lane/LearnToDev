using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Mvc.Models;

public class SchoolLevel
{
    public int Id { get; set; }

    public int SchoolId { get; set; }
    public School School { get; set; } = null!;

    [Range(1, 3)]
    public int Level { get; set; }

    public string? Compendium { get; set; }

    public ICollection<StudyQuestion> StudyQuestions { get; set; } = new List<StudyQuestion>();

    public ICollection<JobRequirement> JobRequirements { get; set; } = new List<JobRequirement>();
}