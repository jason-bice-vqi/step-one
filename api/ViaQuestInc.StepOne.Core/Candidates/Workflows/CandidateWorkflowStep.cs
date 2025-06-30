using System.Text.Json.Serialization;
using ViaQuestInc.StepOne.Core.Data.Entity;
using ViaQuestInc.StepOne.Core.Workflows.Steps;

namespace ViaQuestInc.StepOne.Core.Candidates.Workflows;

public class CandidateWorkflowStep : EntityBase<int>
{
    public required int CandidateWorkflowId { get; set; }

    [JsonIgnore]
    public CandidateWorkflow? CandidateWorkflow { get; set; }

    public required int WorkflowStepId { get; set; }

    public WorkflowStep? WorkflowStep { get; set; }

    public DateTime? CompletedAt { get; set; }

    public bool IsCompleted => CandidateWorkflowStepStatus == CandidateWorkflowStepStatuses.Completed;

    public bool IsConfirmedByAdmin { get; set; }

    public CandidateWorkflowStepStatuses CandidateWorkflowStepStatus { get; set; }
}