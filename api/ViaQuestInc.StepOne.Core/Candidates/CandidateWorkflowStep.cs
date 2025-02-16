using ViaQuestInc.StepOne.Core.Workflows;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Candidates;

public class CandidateWorkflowStep : EntityBase<int>
{
    public required int CandidateWorkflowId { get; set; }
    
    public CandidateWorkflow? CandidateWorkflow { get; set; }
    
    public required int WorkflowStepId { get; set; }
    
    public WorkflowStep? WorkflowStep { get; set; }
    
    public DateTime? CompletedAt { get; set; }
}