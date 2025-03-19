using ViaQuestInc.StepOne.Core.Candidates.Workflows;
using ViaQuestInc.StepOne.Core.Workflows.Steps;
using ViaQuestInc.StepOne.Kernel.Data;

namespace ViaQuestInc.StepOne.Core.Candidates.Services;

public class CandidateService(IRepository repository)
{
    private static readonly string[] CandidateWorkflowIncludes =
    [
        $"{nameof(CandidateWorkflow)}.{nameof(CandidateWorkflow.CandidateWorkflowSteps)}.{nameof(CandidateWorkflowStep.WorkflowStep)}.{nameof(WorkflowStep.Step)}"
    ];
    
    public async Task<Candidate?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        return await repository.GetAsync<Candidate>(x => x.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task RecordAuthenticatedAsync(Candidate candidate, CancellationToken cancellationToken)
    {
        candidate.LastAuthenticatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(candidate, cancellationToken);
    }

    public async Task RecordOtpRequestedAsync(Candidate candidate, CancellationToken cancellationToken)
    {
        candidate.OtpLastRequestedAt = DateTime.UtcNow;

        await repository.UpdateAsync(candidate, cancellationToken);
    }
    
    public async Task<CandidateWorkflow?> GetAsync(Guid candidateId, CancellationToken cancellationToken)
    {
        return await repository.FindAsync<CandidateWorkflow>(candidateId, cancellationToken);
    }
    
    public async Task<Candidate?> GetWithWorkflowAsync(Guid candidateId, CancellationToken cancellationToken)
    {
        return await repository.GetWithChildrenAsync<Candidate>(x => x.Id == candidateId,
            cancellationToken, CandidateWorkflowIncludes);
    }
}