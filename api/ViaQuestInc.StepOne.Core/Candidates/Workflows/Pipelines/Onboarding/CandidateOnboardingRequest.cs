using ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding.Operations;
using ViaQuestInc.StepOne.Core.Kernel;
using ViaQuestInc.StepOne.Core.Organization;
using ViaQuestInc.StepOne.Core.Workflows;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding;

public class CandidateOnboardingRequest
{
    public int CandidateId { get; set; }
    
    [SetByOperation(nameof(InitializeRequestOperation))]
    public Candidate? Candidate { get; set; }
    
    public bool MatchAtsJobTitleToOfficialJobTitle { get; set; }
    
    public bool MatchWorkflowToJobTitle { get; set; }
    
    public int JobTitleId { get; set; }
    
    [SetByOperation(nameof(InitializeRequestOperation))]
    public JobTitle? JobTitle { get; set; }
    
    public bool SendOnboardingInvite { get; set; }
    
    public int WorkflowId { get; set; }
    
    [SetByOperation(nameof(InitializeRequestOperation))]
    public Workflow? Workflow { get; set; }
}