namespace ViaQuestInc.StepOne.Kernel.Data;

public class DatabaseConfig
{
    /// <summary>
    /// Disables database seeding/population: useful for rapid iteration on model changes.
    /// </summary>
    public bool DisablePopulators { get; set; }
    
    public bool EnableMigrations { get; set; }

    public int RowValueExpressionLimit { get; set; }

    public DatabaseStartupTypes DatabaseStartupType { get; set; }
}