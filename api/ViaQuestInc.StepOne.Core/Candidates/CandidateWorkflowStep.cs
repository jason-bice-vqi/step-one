using ViaQuestInc.StepOne.Core.Workflows;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Candidates;

public class CandidateWorkflowStep : EntityBase<int>
{
    public int CandidateWorkflowId { get; set; }
    
    public CandidateWorkflow CandidateWorkflow { get; set; }
    
    public int WorkflowStepId { get; set; }
    
    public WorkflowStep WorkflowStep { get; set; }
}