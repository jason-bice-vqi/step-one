namespace ViaQuestInc.StepOne.Core.Auth.Otp.Services;

public class OtpRequest
{
    public string PhoneNumber { get; set; }
    
    public string? Otp { get; set; }
}