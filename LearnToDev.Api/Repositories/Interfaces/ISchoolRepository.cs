using LearnToDev.Api.Models;

namespace LearnToDev.Api.Repositories.Interfaces;

public interface ISchoolRepository
{
    Task<List<School>> GetAllAsync();
    Task<School?> GetByIdAsync(int id);
    Task<School> AddAsync(School school);
    Task<bool> UpdateAsync(School school);
    Task<bool> DeleteAsync(int id);
    Task<School?> GetByIdWithLevelAsync(int id);
}