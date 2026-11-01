using LearnToDev.Mvc.Models;
namespace LearnToDev.Tests;

public class GameRulesTests
{
    [Theory]
    [InlineData(1, 2, 50)] // Test data for level 1: expects 2 hours and 50 price
    [InlineData(2, 3, 100)]
    [InlineData(3, 4, 200)]
    public void ExamHoursAndPrice_ValidLevel_ReturnsExpected(int level, int hours, int price)
    {
        Assert.Equal(hours, GameRules.ExamHours(level)); // Ensure hours is correct for the level
        Assert.Equal(price, GameRules.ExamPrice(level)); // Ensure price is correct for the level
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    [InlineData(-1)]
    public void ExamHoursAndPrice_InvalidLevels_Throws(int level)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => GameRules.ExamHours(level)); //Sjekker at GameRules.ExamHours kaster ArgumentOutOfRangeException for ugyldige nivåer
        Assert.Throws<ArgumentOutOfRangeException>(() => GameRules.ExamPrice(level));
    }
}