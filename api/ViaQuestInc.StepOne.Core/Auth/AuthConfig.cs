using ViaQuestInc.StepOne.Core.Auth.AzureActiveDirectory;
using ViaQuestInc.StepOne.Core.Auth.Otp.Services.TwilioOtp;
using ViaQuestInc.StepOne.Core.Auth.Services;

namespace ViaQuestInc.StepOne.Core.Auth;

public class AuthConfig
{
    public AzureAdConfig AzureAd { get; set; }

    public JwtConfig Jwt { get; set; }

    public TwilioConfig Twilio { get; set; }
}