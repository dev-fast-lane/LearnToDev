using LearnToDev.Api.Models;

namespace LearnToDev.Api.Repositories.Interfaces;

public interface IJobRepository
{
    Task<List<Job>> GetAllAsync();
    Task<Job?> GetByIdAsync(int id);
    Task<Job> AddAsync(Job job);
    Task<bool> UpdateAsync(Job job);
    Task<bool> DeleteAsync(int id);
    Task<Job?> GetByIdWithRequirementsAsync(int id);
    Task<List<Job>> GetAllWithRequirementsAsync();
}
