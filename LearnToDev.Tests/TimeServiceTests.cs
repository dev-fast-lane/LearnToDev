using LearnToDev.Mvc.Models;
using LearnToDev.Mvc.Services;

namespace LearnToDev.Tests;

public class TimeServiceTests
{
    [Fact]
    public void TrySpendHours_EnoughHours_DeductsHours()
    {
        // Arrange
        var player = new Player { HoursLeftInDay = 10 }; // Set up a player with 10 hours left in the day

        // Act
        var result = TimeService.TrySpendHours(player, 3); // Try to spend 3 hours

        // Assert
        Assert.True(result.Success); // Ensure the operation was successful
        Assert.Equal(7, player.HoursLeftInDay); // Ensure the hours left in the day is now 7 (10 - 3)
    }

    [Fact]
    public void TrySpendHours_TooFewHours_RejectsAndChangesNothing()
    {
        // Arrange
        var player = new Player { HoursLeftInDay = 2 }; // Set up a player with only 2 hours left in the day

        // Act
        var result = TimeService.TrySpendHours(player, 3); // Try to spend 3 hours, which is more than the player has

        // Assert
        Assert.False(result.Success); // Ensure the operation was not successful
        Assert.False(string.IsNullOrWhiteSpace(result.Message)); // Ensure there is an error message
        Assert.Equal(2, player.HoursLeftInDay); // Ensure hours are not deducted
    }

    [Fact]
    public void TrySpendHours_ExactlyEnough_GivesZeroNotNegative()
    {
        var player = new Player { HoursLeftInDay = 4 };

        var result = TimeService.TrySpendHours(player, 4);

        Assert.True(result.Success);
        Assert.Equal(0, player.HoursLeftInDay);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void TrySpendHours_NonPositiveHours_Rejected(int hours)
    {
        var player = new Player { HoursLeftInDay = 5 };

        var result = TimeService.TrySpendHours(player, hours); // Try to spend 0 or negative hours

        Assert.False(result.Success); // Ensure the operation was not successful
        Assert.Equal(5, player.HoursLeftInDay); // Ensure hours are not deducted
    }
}