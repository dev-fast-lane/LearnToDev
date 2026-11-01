namespace LearnToDev.Mvc.Models;
public static class GameRules
{
    //Spilleregler for LearnToDev se [spilltall.md](/LearnToDev/Spilltall.md)
    public const int HoursPerPracticeQuestion = 1;
    public const int StartMoney = 50;
    
    public const int TasksPerWorkDay = 5;
    public const int GoodWorkMark = 3;

    public const int WarningsBeforeFired = 2;
    public const int WarningResetDays = 5;

    public const int ExamQuestionCount = 5;
    public const int ExamPassMark = 4;

    // Tid i timer for hver eksamens level.
    public static int ExamHours(int level) => level switch
    {
        1 => 2,
        2 => 3,
        3 => 4,
        _ => throw new ArgumentOutOfRangeException(nameof(level), "Level must be 1, 2 or 3.")
    };

    // Eksamens pris for hver level.
    public static int ExamPrice(int level) => level switch
    {
        1 => 50,
        2 => 100,
        3 => 200,
        _ => throw new ArgumentOutOfRangeException(nameof(level), "Level must be 1, 2 or 3.")
    };
}