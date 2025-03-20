namespace ViaQuestInc.StepOne.Kernel.Auth;

public static class Claims
{
    private const string ApplicationSchema = "https://step-one.viaquestinc.com/schemas/claims";

    public const string CandidateId = $"{ApplicationSchema}/candidate-id";

    public static class Roles
    {
        public const string Administrator = nameof(Administrator);
    }
}