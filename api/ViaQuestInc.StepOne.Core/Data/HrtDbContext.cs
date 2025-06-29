using Microsoft.EntityFrameworkCore;

namespace ViaQuestInc.StepOne.Core.Data;

public class HrtDbContext(DbContextOptions<HrtDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        throw new("Model creation not allowed from any database other than HR Tracker.");
    }
}