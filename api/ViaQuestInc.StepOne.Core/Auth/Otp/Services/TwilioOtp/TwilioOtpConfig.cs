namespace ViaQuestInc.StepOne.Core.Auth.Otp.Services;

public class TwilioOtpConfig
{
    public string AccountSid { get; set; }
    
    public string AuthToken { get; set; }
    
    /// <summary>
    /// The Service SSID.
    /// </summary>
    /// <remarks>https://console.twilio.com/us1/develop/verify/services</remarks>
    public string VerifyServiceSid { get; set; }
    
}