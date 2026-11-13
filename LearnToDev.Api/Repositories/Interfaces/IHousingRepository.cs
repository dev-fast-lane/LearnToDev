using LearnToDev.Api.Models;

namespace LearnToDev.Api.Repositories.Interfaces;

public interface IHousingRepository
{
    Task<List<Housing>> GetAllAsync();
    Task<Housing?> GetByIdAsync(int id);
    Task<Housing> AddAsync(Housing housing);
    Task<bool> UpdateAsync(Housing housing);
    Task<bool> DeleteAsync(int id);
    Task<bool> OrderExistsAsync(int order, int? excludeId = null);
    Task<bool> HasPlayersAsync(int id);
}