using LearnToDev.Api.Models;

namespace LearnToDev.Api.Repositories.Interfaces;

public interface ISchoolLevelRepository
{
    Task<List<SchoolLevel>> GetAllAsync();
    Task<SchoolLevel?> GetByIdAsync(int id);
    Task<SchoolLevel> AddAsync(SchoolLevel schoolLevel);
    Task<bool> UpdateAsync(SchoolLevel schoolLevel);
    Task<bool> DeleteAsync(int id);

}