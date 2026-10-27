using System.ComponentModel.DataAnnotations;

namespace LearnToDev.Mvc.Models;

public class School
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    //True only for Fullstack: it has an exam only, and no compendium
    public bool IsExamOnly { get; set; }

    //The levels in the school and the players' progress in it (one school has many of each)
    public ICollection<SchoolLevel> SchoolLevels { get; set; } = new List<SchoolLevel>();
    public ICollection<PlayerSchoolProgress> PlayerSchoolProgresses { get; set; } = new List<PlayerSchoolProgress>();
}