namespace ViaQuestInc.StepOne.Core.Candidates.Workflows;

public enum CandidateWorkflowStepStatuses
{
    Blocked = -1,
    PendingCandidate = 0,
    PendingAdministratorReview = 1,
    Completed = 2
}