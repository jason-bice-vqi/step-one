using ViaQuestInc.StepOne.Core.Candidates.Workflows;
using ViaQuestInc.StepOne.Kernel.Data;

namespace ViaQuestInc.StepOne.Core.Candidates.Services;

public class CandidateSearchRequest : SearchRequestBase
{
    public string? Name { get; set; }
    
    public string? JobTitle { get; set; }
    
    public CandidateStatuses? CandidateStatus { get; set; }
    
    public CandidateWorkflowStepStatuses? CandidateWorkflowStepStatus { get; set; }
}