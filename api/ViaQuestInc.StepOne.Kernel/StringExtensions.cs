using System.Text.RegularExpressions;

namespace ViaQuestInc.StepOne.Kernel;

public static partial class StringExtensions
{
    private static readonly Regex EmailRegex = MyRegex();
    
    private static readonly Regex PhoneRegex = MyRegex1();

    public static InputTypes GetInputType(this string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);
        
        if (EmailRegex.IsMatch(input)) return InputTypes.Email;
        
        if (PhoneRegex.IsMatch(input)) return InputTypes.PhoneNumber;
        
        return InputTypes.Indeterminate;
    }

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
    
    [GeneratedRegex(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", RegexOptions.Compiled)]
    private static partial Regex MyRegex1();
}