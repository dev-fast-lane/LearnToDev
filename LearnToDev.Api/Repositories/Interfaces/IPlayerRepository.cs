using LearnToDev.Api.Models;

namespace LearnToDev.Api.Repositories.Interfaces;

public interface IPlayerRepository
{
    Task<Player?> GetByIdWithDetailsAsync(int id);
    Task<Player?> GetByIdWithSchoolProgressAsync(int id);
    Task<List<Player>> GetByUserIdAsync(string userId);
    Task<Player> AddAsync(Player player);
    Task<bool> UpdateAsync(Player player);
    Task<List<JobDay>> GetCompletedJobDaysAsync(int playerId, int day);
    Task<HashSet<int>> GetCompletedJobIdsAsync(int playerId);
    Task SaveSchoolProgressAsync(int playerId, int schoolId, int levelPassed);
    Task<JobDay> AddJobDayAsync(JobDay jobDay);
}
