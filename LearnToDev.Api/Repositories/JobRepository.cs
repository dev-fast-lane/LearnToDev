using LearnToDev.Api.Data;
using LearnToDev.Api.Models;
using LearnToDev.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearnToDev.Api.Repositories;

public class JobRepository : IJobRepository
{

    private readonly ApplicationDbContext _context;
    public JobRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Job> AddAsync(Job job)
    {
        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();
        return job;
    }

    //Throws DbUpdateException if another job requires this one (DeleteBehavior.Restrict on JobPrerequisite)
    public async Task<bool> DeleteAsync(int id)
    {
        var toBeDeleted = await _context.Jobs.FindAsync(id);

        if (toBeDeleted == null) return false;

        _context.Jobs.Remove(toBeDeleted);
        await _context.SaveChangesAsync();
        return true;
    }

    //Sorted by Rank so the list follows the job ladder
    public async Task<List<Job>> GetAllAsync()
    {
        return await _context.Jobs.OrderBy(j => j.Rank).ToListAsync();
    }

    public async Task<Job?> GetByIdAsync(int id)
    {
        return await _context.Jobs.FindAsync(id);
    }

    public async Task<Job?> GetByIdWithRequirementsAsync(int id)
    {
        return await WithRequirements().FirstOrDefaultAsync(j => j.Id == id);
    }

    //All jobs with requirements, so JobEligibilityService can find the lock reasons for every job at once (#36)
    public async Task<List<Job>> GetAllWithRequirementsAsync()
    {
        return await WithRequirements().OrderBy(j => j.Rank).ToListAsync();
    }

    public async Task<bool> UpdateAsync(Job job)
    {
        var oldJob = await _context.Jobs.FindAsync(job.Id);

        if (oldJob == null) return false;

        _context.Entry(oldJob).CurrentValues.SetValues(job);
        await _context.SaveChangesAsync();
        return true;
    }

    //The required school levels with their school (for names like "Fullstack 2"),
    //and the jobs the player must have worked in first
    private IQueryable<Job> WithRequirements()
    {
        return _context.Jobs
            .Include(j => j.JobRequirements)
                .ThenInclude(r => r.SchoolLevel)
                    .ThenInclude(l => l.School)
            .Include(j => j.Prerequisites)
                .ThenInclude(p => p.RequiredJob);
    }
}
