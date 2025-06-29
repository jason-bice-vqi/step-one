namespace ViaQuestInc.StepOne.Core.Data;

/// <summary>
/// Describes an application's database handling at start-up.
/// </summary>
public enum DatabaseStartupTypes
{
    /// <summary>
    /// Indicates that no action should be taken with the database upon start-up.
    ///
    /// This is the preferred setting in all deployed environments. Migrations should be applied outside of
    /// (and prior to) application start-up.
    /// </summary>
    NoAction,

    /// <summary>
    /// Indicates the database should be deleted and recreated at startup.
    /// </summary>
    Recreate,

    /// <summary>
    /// Indicates the database should be preserved and should have pending migrations automatically applied. If the
    /// database does not exist, it will be created.
    ///
    /// This should be avoided in deployed environments; it is intended to be used in local development environments
    /// when it is desirable to retain an existing database and apply pending migrations that have been acquired through
    /// source control.
    /// </summary>
    ApplyMigrations
}