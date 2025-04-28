using System.Text.RegularExpressions;

namespace ViaQuestInc.StepOne.Kernel;

public static partial class StringExtensions
{
    private static readonly Regex EmailRegex = CompiledEmailRegex();
    
    private static readonly Regex PhoneRegex = CompiledPhoneNumberRegex();

    public static InputTypes GetInputType(this string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);
        
        if (EmailRegex.IsMatch(input)) return InputTypes.Email;
        
        if (PhoneRegex.IsMatch(input)) return InputTypes.PhoneNumber;
        
        return InputTypes.Indeterminate;
    }

    public static string? ToUnformattedPhoneNumber(this string input)
    {
        var match = CompiledFormattedPhoneNumberRegex().Match(input);

        if (match.Success) return match.Groups[2].Value + match.Groups[3].Value + match.Groups[4].Value;

        return null;
    }

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled)]
    private static partial Regex CompiledEmailRegex();
    
    [GeneratedRegex(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", RegexOptions.Compiled)]
    private static partial Regex CompiledPhoneNumberRegex();
    
    [GeneratedRegex(@"^(\+?1[\s\-.]?)?\(?(\d{3})\)?[\s\-.]?(\d{3})[\s\-.]?(\d{4})$")]
    private static partial Regex CompiledFormattedPhoneNumberRegex();
}