﻿using ViaQuestInc.StepOne.Core.Candidates.Workflows;
using ViaQuestInc.StepOne.Core.Data;

namespace ViaQuestInc.StepOne.Core.Candidates.Services;

public class CandidateSearchRequest : SearchRequestBase
{
    public string? Name { get; set; }

    public string? JobTitle { get; set; }

    public CandidateStatuses? CandidateStatus { get; set; }
    
    public CandidateWorkflowStatus? CandidateWorkflowStatus { get; set; }

    public CandidateWorkflowStepStatuses? CandidateWorkflowStepStatus { get; set; }
}