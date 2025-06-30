using Microsoft.EntityFrameworkCore;
using ViaQuestInc.StepOne.Core.Organization.HrTracker;

namespace ViaQuestInc.StepOne.Core.Data;

public class HrtDbContext(DbContextOptions<HrtDbContext> options) : DbContext(options)
{
    public DbSet<HrtBranch> Branches { get; set; }
    
    public DbSet<HrtCompany> Companies { get; set; }
    
    public DbSet<HrtJobTitle> JobTitles { get; set; }
    
    public DbSet<HrtRegion> Regions { get; set; }
}