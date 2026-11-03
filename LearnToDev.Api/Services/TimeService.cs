using LearnToDev.Mvc.Models;
namespace LearnToDev.Mvc.Services;

public record TimeSpendResult(bool Success, string? Message = null); // Record type to represent the result of trying to spend hours

public static class TimeService
{
    public static TimeSpendResult TrySpendHours(Player player, int hours) // Method to attempt to spend a certain number of hours for a player
    {
        if (hours <= 0)
            return new TimeSpendResult(false, "Antall timer må være større enn 0."); // Reject if the number of hours is not positive

        if (hours > player.HoursLeftInDay)
            return new TimeSpendResult(false, $"Du har ikke nok timer igjen i dagen (trenger {hours}, du har {player.HoursLeftInDay})."); // Reject if the player does not have enough hours left in the day

        player.HoursLeftInDay -= hours;
        return new TimeSpendResult(true); // Deduct the hours and return a successful result
    }
}