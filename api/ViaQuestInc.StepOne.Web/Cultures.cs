using System.Globalization;

namespace ViaQuestInc.StepOne.Web;

/// <summary>
/// Static methods pertaining to <see cref="CultureInfo"/>.
/// </summary>
public static class Cultures
{
    /// <summary>
    /// Sets the system-wide culture.
    /// </summary>
    /// <param name="name">The name of the culture to set.</param>
    public static void SetSystemCulture(string name)
    {
        var cultureInfo = new CultureInfo(name);
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }
}