using LearnToDev.Api.Data;
using LearnToDev.Api.Models;
using LearnToDev.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearnToDev.Api.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly ApplicationDbContext _context;

    //Dependency injection: Program.cs gir oss DbContext for forespørselen
    public PlayerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    //Spilleren med bolig, nåværende jobb og skolefremgang. Returnerer null hvis ingen spiller har denne id-en
    public async Task<Player?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Players
            .Include(p => p.Housing)
            .Include(p => p.CurrentJob)
            .Include(p => p.SchoolProgresses)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    //Spilleren med bare skolefremgang, til JobEligibilityService som ikke trenger bolig eller jobb (#36)
    public async Task<Player?> GetByIdWithSchoolProgressAsync(int id)
    {
        return await _context.Players
            .Include(p => p.SchoolProgresses)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    //Alle spillerne på én konto, til startskjermen (#15)
    public async Task<List<Player>> GetByUserIdAsync(string userId)
    {
        return await _context.Players
            .Where(p => p.UserId == userId)
            .OrderBy(p => p.Id)
            .ToListAsync();
    }

    //Databasen setter Id når vi lagrer, så objektet som returneres har den nye Id-en
    public async Task<Player> AddAsync(Player player)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync();
        return player;
    }

    //Returnerer false hvis spilleren ikke finnes, så controlleren kan svare 404
    public async Task<bool> UpdateAsync(Player player)
    {
        var existing = await _context.Players.FindAsync(player.Id);
        if (existing == null)
            return false;

        //Kopierer verdiene fra det innkommende objektet over på det EF allerede følger
        _context.Entry(existing).CurrentValues.SetValues(player);
        await _context.SaveChangesAsync();
        return true;
    }

    //Fullførte jobbdager på én dag, med jobben, så lønnen kan betales ut ved ny dag (#43)
    public async Task<List<JobDay>> GetCompletedJobDaysAsync(int playerId, int day)
    {
        return await _context.JobDays
            .Include(d => d.Job)
            .Where(d => d.PlayerId == playerId && d.Day == day && d.Completed)
            .ToListAsync();
    }

    //Id-ene til jobbene spilleren har fullført minst én dag i (#36). HashSet fordi den brukes til raske Contains-sjekker
    public async Task<HashSet<int>> GetCompletedJobIdsAsync(int playerId)
    {
        return await _context.JobDays
            .Where(d => d.PlayerId == playerId && d.Completed)
            .Select(d => d.JobId)
            .ToHashSetAsync();
    }

    //Oppretter raden første gang spilleren består et nivå på en skole, og oppdaterer den etter det
    public async Task SaveSchoolProgressAsync(int playerId, int schoolId, int levelPassed)
    {
        var progress = await _context.PlayerSchoolProgresses.FindAsync(playerId, schoolId);

        if (progress == null)
        {
            _context.PlayerSchoolProgresses.Add(new PlayerSchoolProgress
            {
                PlayerId = playerId,
                SchoolId = schoolId,
                LevelPassed = levelPassed
            });
        }
        else
        {
            progress.LevelPassed = levelPassed;
        }

        await _context.SaveChangesAsync();
    }

    //Databasen setter Id når vi lagrer, så objektet som returneres har den nye Id-en
    public async Task<JobDay> AddJobDayAsync(JobDay jobDay)
    {
        _context.JobDays.Add(jobDay);
        await _context.SaveChangesAsync();
        return jobDay;
    }
}
