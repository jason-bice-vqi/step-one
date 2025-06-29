namespace ViaQuestInc.StepOne.Core.Auth.Services;

public class JwtConfig
{
    public string Audience { get; set; }

    public string Issuer { get; set; }

    public string Key { get; set; }

    public int LifetimeHoursInternal { get; set; }

    public int LifetimeHoursExternal { get; set; }
}