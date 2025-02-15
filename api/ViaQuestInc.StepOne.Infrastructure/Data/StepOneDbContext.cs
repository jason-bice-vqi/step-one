﻿using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using ViaQuestInc.StepOne.Core.Candidates;
using ViaQuestInc.StepOne.Core.Workflows;
using ViaQuestInc.StepOne.Kernel.Data;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Infrastructure.Data;

public class StepOneDbContext(DbContextOptions<StepOneDbContext> options) : DbContext(options)
{
    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<CandidateWorkflow> CandidateWorkflows { get; set; }
    public DbSet<CandidateWorkflowStep> CandidateWorkflowSteps { get; set; }
    public DbSet<Workflow> Workflows { get; set; }
    public DbSet<WorkflowStep> WorkflowSteps { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.ConfigureWarnings(x => x.Ignore(RelationalEventId.MultipleCollectionIncludeWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Candidate to CandidateWorkflow (One-to-One)
        modelBuilder.Entity<Candidate>()
            .HasOne(c => c.CandidateWorkflow)
            .WithOne(cw => cw.Candidate)
            .HasForeignKey<CandidateWorkflow>(cw => cw.CandidateId)
            .OnDelete(DeleteBehavior.Cascade); // Deleting Candidate deletes CandidateWorkflow

        // CandidateWorkflow to Workflow (Many-to-One)
        modelBuilder.Entity<CandidateWorkflow>()
            .HasOne(cw => cw.Workflow)
            .WithMany()
            .HasForeignKey(cw => cw.WorkflowId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent multiple cascading paths

        // CandidateWorkflowStep to WorkflowStep (Many-to-One)
        modelBuilder.Entity<CandidateWorkflowStep>()
            .HasOne(cws => cws.WorkflowStep)
            .WithMany()
            .HasForeignKey(cws => cws.WorkflowStepId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cycles

        // CandidateWorkflowStep to CandidateWorkflow (Many-to-One)
        modelBuilder.Entity<CandidateWorkflowStep>()
            .HasOne(cws => cws.CandidateWorkflow)
            .WithMany(cw => cw.CandidateWorkflowSteps)
            .HasForeignKey(cws => cws.CandidateWorkflowId)
            .OnDelete(DeleteBehavior.Cascade); // Deleting CandidateWorkflow deletes its CandidateWorkflowSteps
    }
    
    public static async Task PopulateDatabaseAsync(IServiceProvider serviceProvider)
    {
        var hostEnvironment = serviceProvider.GetRequiredService<IHostEnvironment>();

        try
        {
            var priorityDatabasePopulators = new IDatabasePopulator[]
            {
                
            };

            Log.Information("Beginning priority seeding of database");

            using (var scope = serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IRepository>();
                var batchSize = scope.ServiceProvider
                    .GetRequiredService<IOptions<DatabaseConfig>>()
                    .Value.RowValueExpressionLimit;

                foreach (var databasePopulator in priorityDatabasePopulators)
                {
                    await PopulateEntity(databasePopulator, repository, serviceProvider, batchSize);
                }
            }

            // Organize items within each group as largest -> smallest
            var parallelSeedingGroups = new[]
            {
                [
                    
                ],
                [
                    
                ],
                [
                    
                ],
                [
                    
                ],
                new IDatabasePopulator[]
                {
                    
                }
            };

            for (var i = 0; i < parallelSeedingGroups.Length; i++)
            {
                ParallelSeedDatabase(serviceProvider, parallelSeedingGroups[i], i);
            }
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Error populating database with default values");
            Environment.Exit(-1);
        }
    }

    private static void ParallelSeedDatabase(
        IServiceProvider serviceProvider,
        IEnumerable<IDatabasePopulator> databasePopulators,
        int tier)
    {
        Log.Information($"Beginning parallel seeding of database - Tier {tier}");

        Parallel.ForEach(
            databasePopulators,
            databasePopulator =>
            {
                using var scope = serviceProvider.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IRepository>();
                var batchSize = scope.ServiceProvider
                    .GetRequiredService<IOptions<DatabaseConfig>>()
                    .Value.RowValueExpressionLimit;

                // Must use Wait() and avoid using an async lambda, which is converted to async void
                // See https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming#avoid-async-void
                PopulateEntity(databasePopulator, repository, serviceProvider, batchSize).Wait();
            }
        );
    }

    private static async Task PopulateEntity(
        IDatabasePopulator databasePopulator,
        IRepository repository,
        IServiceProvider serviceProvider,
        int batchSize)
    {
        if (!await databasePopulator.ShouldExecuteAsync(CancellationToken.None))
        {
            Log.Information($"Skipping [{databasePopulator.GetType().Name}].");

            return;
        }

        Log.Information($"Executing [{databasePopulator.GetType().Name}]...");
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        await databasePopulator.PopulateAsync(repository, serviceProvider, batchSize, default);
        stopwatch.Stop();
        Log.Information($"[{databasePopulator.GetType().Name}] executed in {stopwatch.ElapsedMilliseconds}ms");
    }
}