using LearnToDev.Api.Data;
using LearnToDev.Api.Models;
using LearnToDev.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace LearnToDev.Api.Repositories;

public class SchoolRepository : ISchoolRepository
{

    private readonly ApplicationDbContext _context;

    public SchoolRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<School> AddAsync(School school)
    {
        _context.Schools.Add(school);
        await _context.SaveChangesAsync();
        return school;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var toBeDeleted = await _context.Schools.FindAsync(id);

        if (toBeDeleted == null) return false;

        _context.Schools.Remove(toBeDeleted);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<School>> GetAllAsync()
    {
        return await _context.Schools.ToListAsync();
    }

    public async Task<School?> GetByIdAsync(int id)
    {
        return await _context.Schools.FindAsync(id);

    }

    public async Task<School?> GetByIdWithLevelAsync(int id)
    {
        return await _context.Schools.Include(s => s.SchoolLevels).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<bool> UpdateAsync(School school)
    {
        var oldSchool = await _context.Schools.FindAsync(school.Id);

        if (oldSchool == null) return false;

        _context.Entry(oldSchool).CurrentValues.SetValues(school);
        await _context.SaveChangesAsync();
        return true;
    }
}