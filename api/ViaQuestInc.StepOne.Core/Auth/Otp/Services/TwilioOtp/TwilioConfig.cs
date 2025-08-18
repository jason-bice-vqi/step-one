namespace ViaQuestInc.StepOne.Core.Auth.Otp.Services.TwilioOtp;

public class TwilioConfig
{
    /// <summary>
    /// The account SSID.
    /// </summary>
    public string AccountSid { get; set; }

    /// <summary>
    /// The Twilio account auth token / password.
    /// </summary>
    public string AuthToken { get; set; }
    
    /// <summary>
    /// The From address to be used for SMS messages.
    /// </summary>
    public string SmsFrom { get; set; }

    /// <summary>
    /// The Service SSID.
    /// </summary>
    /// <remarks>https://console.twilio.com/us1/develop/verify/services</remarks>
    public string VerifyServiceSid { get; set; }
}