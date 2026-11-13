namespace LearnToDev.Api.DTOs;
public class StudyQuestionDto
{
    public int Id { get; set; }
    public int SchoolLevelId { get; set; }
    public string SchoolName { get; set; } = string.Empty;
    public int Level { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public List<AnswerOptionDto> AnswerOptions { get; set; } = new();
}