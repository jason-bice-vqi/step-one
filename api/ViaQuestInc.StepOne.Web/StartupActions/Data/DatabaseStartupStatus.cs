namespace ViaQuestInc.StepOne.Web.StartupActions.Data;

public static class DatabaseStartupStatus
{
    /// <summary>
    /// Indicates whether the database needs to be recreated during the current start-up procedure.
    /// </summary>
    public static bool IsNew { get; set; }
}