using LearnToDev.Api.Data;
using LearnToDev.Api.Models;
using LearnToDev.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearnToDev.Api.Repositories;

public class HousingRepository : IHousingRepository
{
    private readonly ApplicationDbContext _context;

    //DI Dependency Injection: Program.cs gives us the DbContext for the current request
    public HousingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    //Sorted by Order so the list goes from Telt to Penthouse
    public async Task<List<Housing>> GetAllAsync()
    {
        return await _context.Housings
            .OrderBy(h => h.Order)
            .ToListAsync();
    }

    //Returns null if no housing has this id
    public async Task<Housing?> GetByIdAsync(int id)
    {
        return await _context.Housings.FindAsync(id);
    }

    //The database sets Id when we save, so the returned object has the new Id
    public async Task<Housing> AddAsync(Housing housing)
    {
        _context.Housings.Add(housing);
        await _context.SaveChangesAsync();
        return housing;
    }

    //Returns false if the housing does not exist, so the controller can answer 404
    public async Task<bool> UpdateAsync(Housing housing)
    {
        var existing = await _context.Housings.FindAsync(housing.Id);
        if (existing == null)
            return false;

        //Copies the values from the incoming object onto the one EF already tracks
        _context.Entry(existing).CurrentValues.SetValues(housing);
        await _context.SaveChangesAsync();
        return true;
    }

    //Returns false if the housing does not exist.
    //Throws DbUpdateException if players live here (DeleteBehavior.Restrict)
    public async Task<bool> DeleteAsync(int id)
    {
        var housing = await _context.Housings.FindAsync(id);
        if (housing == null)
            return false;

        _context.Housings.Remove(housing);
        await _context.SaveChangesAsync();
        return true;
    }
}
