using ViaQuestInc.StepOne.Core.Workflows;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Candidates;

public class CandidateWorkflow : EntityBase<int>, IEntityStatusAssignable
{
    public int CandidateId { get; set; }
    
    public Candidate Candidate { get; set; }
    
    public int WorkflowId { get; set; }
    
    public Workflow Workflow { get; set; }
    
    public EntityStatuses EntityStatus { get; set; }
    
    public ICollection<CandidateWorkflowStep> CandidateWorkflowSteps { get; set; }
}