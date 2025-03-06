namespace ViaQuestInc.StepOne.Core.Auth.Otp.Services.TwilioOtp;

public class OtpRequest
{
    public string PhoneNumber { get; set; }
    
    public string? Otp { get; set; }
}