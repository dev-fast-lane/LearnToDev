using LearnToDev.Api.Models;

namespace LearnToDev.Api.Repositories.Interfaces;

public interface IJobTaskRepository
{
    Task<List<JobTask>> GetAllAsync();
    Task<JobTask?> GetByIdAsync(int id);
    Task<JobTask> AddAsync(JobTask jobTask);
    Task<bool> UpdateAsync(JobTask jobTask);
    Task<bool> DeleteAsync(int id);
    Task<JobTask?> GetByIdWithAnswersAsync(int id);
}
