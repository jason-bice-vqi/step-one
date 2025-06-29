using Microsoft.EntityFrameworkCore;
using ViaQuestInc.StepOne.Core.Organization.Hrt;

namespace ViaQuestInc.StepOne.Core.Data;

public class HrtDbContext(DbContextOptions<HrtDbContext> options) : DbContext(options)
{
    public DbSet<HrtCompany> Companies { get; set; }
}