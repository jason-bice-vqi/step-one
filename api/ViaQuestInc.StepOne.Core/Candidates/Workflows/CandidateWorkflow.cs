using System.Text.Json.Serialization;
using ViaQuestInc.StepOne.Core.Data.Entity;
using ViaQuestInc.StepOne.Core.Workflows;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows;

public class CandidateWorkflow
    : EntityBase<int>,
        IEntityStatusAssignable
{
    public required int CandidateId { get; set; }

    [JsonIgnore]
    public Candidate? Candidate { get; set; }

    public required int WorkflowId { get; set; }
    
    public Workflow? Workflow { get; set; }

    public required DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public required EntityStatuses EntityStatus { get; set; }

    public ICollection<CandidateWorkflowStep>? CandidateWorkflowSteps { get; set; }

    public int? CompletedSteps => CandidateWorkflowSteps?.Count(x => x.IsCompleted);

    public int? PercentComplete
    {
        get
        {
            if (CandidateWorkflowSteps == null) return null;

            return Convert.ToInt32(100 * Math.Round((double)CompletedSteps! / CandidateWorkflowSteps.Count, 3));
        }
    } 
}