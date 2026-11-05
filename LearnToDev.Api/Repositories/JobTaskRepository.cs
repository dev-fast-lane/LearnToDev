using LearnToDev.Api.Data;
using LearnToDev.Api.Models;
using LearnToDev.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearnToDev.Api.Repositories;

public class JobTaskRepository : IJobTaskRepository
{

    private readonly ApplicationDbContext _context;
    public JobTaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JobTask> AddAsync(JobTask jobTask)
    {
        _context.JobTasks.Add(jobTask);
        await _context.SaveChangesAsync();
        return jobTask;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var toBeDeleted = await _context.JobTasks.FindAsync(id);

        if (toBeDeleted == null) return false;

        _context.JobTasks.Remove(toBeDeleted);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<JobTask>> GetAllAsync()
    {
        return await _context.JobTasks.ToListAsync();
    }

    public async Task<JobTask?> GetByIdAsync(int id)
    {
        return await _context.JobTasks.FindAsync(id);
    }

    //The approved answers are needed to check what the player wrote (#39)
    public async Task<JobTask?> GetByIdWithAnswersAsync(int id)
    {
        return await _context.JobTasks.Include(t => t.JobTaskAnswers).FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<bool> UpdateAsync(JobTask jobTask)
    {
        var oldJobTask = await _context.JobTasks.FindAsync(jobTask.Id);

        if (oldJobTask == null) return false;

        _context.Entry(oldJobTask).CurrentValues.SetValues(jobTask);
        await _context.SaveChangesAsync();
        return true;
    }
}
