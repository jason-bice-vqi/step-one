using System.ComponentModel.DataAnnotations;
using ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding.Operations;
using ViaQuestInc.StepOne.Core.Kernel;
using ViaQuestInc.StepOne.Core.Organization;
using ViaQuestInc.StepOne.Core.Workflows;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows.Pipelines.Onboarding;

public class CandidateOnboardingRequest
{
    [SetByController]
    public int? CandidateId { get; set; }
    
    [SetByOperation(nameof(InitializeRequestOperation))]
    public Candidate? Candidate { get; set; }
    
    [Required]
    public bool CreateJobTitleAlias { get; set; }
    
    [Required]
    public bool AssignWorkflowToJobTitle { get; set; }
    
    [Required]
    public int JobTitleId { get; set; }
    
    [SetByOperation(nameof(InitializeRequestOperation))]
    public JobTitle? JobTitle { get; set; }
    
    [Required]
    public bool SendOnboardingInvite { get; set; }
    
    [Required]
    public int WorkflowId { get; set; }
    
    [SetByOperation(nameof(InitializeRequestOperation))]
    public Workflow? Workflow { get; set; }
}