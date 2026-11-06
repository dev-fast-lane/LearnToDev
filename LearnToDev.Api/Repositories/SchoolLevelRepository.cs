using LearnToDev.Api.Data;
using LearnToDev.Api.Models;
using LearnToDev.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearnToDev.Api.Repositories;

public class SchoolLevelRepository : ISchoolLevelRepository
{
    private readonly ApplicationDbContext _context;
    public SchoolLevelRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SchoolLevel> AddAsync(SchoolLevel schoolLevel)
    {
        _context.SchoolLevels.Add(schoolLevel);
        await _context.SaveChangesAsync();
        return schoolLevel;

    }

    public async Task<bool> DeleteAsync(int id)
    {
        var oldSchoolLevel = await _context.SchoolLevels.FindAsync(id);

        if (oldSchoolLevel == null) return false;

        _context.SchoolLevels.Remove(oldSchoolLevel);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<SchoolLevel>> GetAllAsync()
    {
        return await _context.SchoolLevels.ToListAsync();
    }

    public async Task<SchoolLevel?> GetByIdAsync(int id)
    {
        return await _context.SchoolLevels.FindAsync(id);
    }

    public async Task<bool> UpdateAsync(SchoolLevel schoolLevel)
    {
        var oldSchoolLevel = await _context.SchoolLevels.FindAsync(schoolLevel.Id);

        if (oldSchoolLevel == null) return false;
        
        _context.Entry(oldSchoolLevel).CurrentValues.SetValues(schoolLevel);
        await _context.SaveChangesAsync();
        return true;
    }
}