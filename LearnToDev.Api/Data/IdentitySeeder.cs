using LearnToDev.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace LearnToDev.Api.Data;

// Creates the roles and the test users. Checks each one first, so it is safe to run on every start
public static class IdentitySeeder
{
    // Test users for the group and the examiner. Only for testing, the passwords are in the README
    private const string AdminUserName = "admin";
    private const string AdminEmail = "admin@learntodev.no";
    private const string AdminPassword = "Admin123!";

    private const string PlayerUserName = "player";
    private const string PlayerEmail = "player@learntodev.no";
    private const string PlayerPassword = "Player123!";

    public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ILogger logger)
    {
        // 1. Roles
        foreach (var role in new[] { Roles.Admin, Roles.Player })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                logger.LogInformation("Created role {Role}", role);
            }
        }

        // 2. Test users
        await CreateUserAsync(userManager, logger, AdminUserName, AdminEmail, AdminPassword, Roles.Admin);
        await CreateUserAsync(userManager, logger, PlayerUserName, PlayerEmail, PlayerPassword, Roles.Player);
    }

    // Creates one user with one role, unless a user with that name already exists
    private static async Task CreateUserAsync(UserManager<IdentityUser> userManager, ILogger logger,
        string userName, string email, string password, string role)
    {
        if (await userManager.FindByNameAsync(userName) != null)
            return;

        var user = new IdentityUser { UserName = userName, Email = email };

        // UserManager hashes the password and checks the password rules
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            logger.LogError("Could not create test user with role {Role}: {Errors}",
                role, string.Join(", ", result.Errors.Select(e => e.Description)));
            return;
        }

        await userManager.AddToRoleAsync(user, role);
        logger.LogInformation("Created test user {UserId} with role {Role}", user.Id, role);
    }
}