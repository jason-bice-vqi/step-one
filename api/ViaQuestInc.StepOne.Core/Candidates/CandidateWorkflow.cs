using ViaQuestInc.StepOne.Core.Workflows;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Candidates;

public class CandidateWorkflow : EntityBase<int>, IEntityStatusAssignable
{
    public required int CandidateId { get; set; }
    
    public Candidate? Candidate { get; set; }
    
    public required int WorkflowId { get; set; }
    
    public Workflow? Workflow { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }
    
    public required EntityStatuses EntityStatus { get; set; }
    
    public ICollection<CandidateWorkflowStep>? CandidateWorkflowSteps { get; set; }
}