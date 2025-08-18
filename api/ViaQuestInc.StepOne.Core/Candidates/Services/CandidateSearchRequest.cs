using ViaQuestInc.StepOne.Core.Candidates.Workflows;
using ViaQuestInc.StepOne.Core.Data;

namespace ViaQuestInc.StepOne.Core.Candidates.Services;

public class CandidateSearchRequest : SearchRequestBase
{
    public string? Name { get; set; }
    
    public int? CompanyId { get; set; }
    
    public int? JobTitleId { get; set; }

    public string? AtsJobTitle { get; set; }
    
    public CandidateWorkflowStatus? CandidateWorkflowStatus { get; set; }
}