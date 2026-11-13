namespace LearnToDev.Api.DTOs;

public class QuestionOptionsDto
{
    public List<SchoolOptionDto> Schools { get; set; } = new();
    public List<SchoolLevelOptionDto> SchoolLevels { get; set; } = new();
}

public class SchoolOptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class SchoolLevelOptionDto
{
    public int Id { get; set; }
    public int SchoolId { get; set; }
    public int Level { get; set; }
}
