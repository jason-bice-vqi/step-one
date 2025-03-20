using System.Reflection;
using System.Text;

namespace ViaQuestInc.StepOne.Kernel;

public static class AssemblyExtensions
{
    public static async Task<string> GetManifestResourceString(this Assembly assembly, string resource,
        Encoding encoding)
    {
        await using var stream = assembly.GetManifestResourceStream(resource);

        if (stream == null) throw new($"The specified fixture file {resource} could not be found.");

        using var streamReader = new StreamReader(stream, encoding);
        
        return await streamReader.ReadToEndAsync();
    }
}