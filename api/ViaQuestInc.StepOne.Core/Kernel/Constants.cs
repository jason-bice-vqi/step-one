namespace ViaQuestInc.StepOne.Core.Kernel;

public static class Constants
{
    private const string ApiKey = "e6b101c3-e633-407c-bccc-c0565ecb4393";

    public static bool AuthorizeApiKey(string apiKeyChallenge)
    {
        return apiKeyChallenge == ApiKey;
    }
}