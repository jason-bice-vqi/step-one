using System.Data;
using Microsoft.EntityFrameworkCore;
using ViaQuestInc.StepOne.Core.Candidates.Import;
using ViaQuestInc.StepOne.Core.Data;
using ViaQuestInc.StepOne.Core.Kernel.Services;
using ViaQuestInc.StepOne.Core.Organization;
using ViaQuestInc.StepOne.Core.Workflows;
using ViaQuestInc.StepOne.Core.Workflows.Steps;

namespace ViaQuestInc.StepOne.Core.Candidates.Services;

public class CandidateService(
    CandidateImportEngine candidateImportEngine,
    ExcelService excelService,
    IRepository<StepOneDbContext> repository
)
{
    private const int HeaderRow = 7;

    private static readonly string[] DefaultIncludes =
    [
        $"{nameof(Candidate.JobTitle)}.{nameof(JobTitle.Company)}",
        $"{nameof(Candidate.JobTitle)}.{nameof(JobTitle.Workflow)}.{nameof(Workflow.WorkflowSteps)}.{nameof(WorkflowStep.Step)}"
    ];

    public async Task<Candidate?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        return await repository.GetAsync<Candidate>(x => x.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<ICollection<Candidate>> ImportAsync(
        MemoryStream candidatesMemoryStream,
        CancellationToken cancellationToken)
    {
        using var dataSet = await excelService.ToDataSetAsync(candidatesMemoryStream, HeaderRow, cancellationToken);

        var hireDateBegin = new DateTime(2025, 3, 1);

        var rawCandidateDataRows = dataSet.Tables[0]
            .AsEnumerable()
            .Where(
                x => DateTime.Parse(
                    x["Date Hired"]
                        .ToString()!) >= hireDateBegin);

        await candidateImportEngine.ImportAsync(rawCandidateDataRows, cancellationToken);

        return null; // todo
    }

    public async Task RecordAuthenticatedAsync(Candidate candidate, CancellationToken cancellationToken)
    {
        candidate.LastAuthenticatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(candidate, cancellationToken);
    }

    public async Task RecordOtpRequestedAsync(Candidate candidate, CancellationToken cancellationToken)
    {
        candidate.OtpLastRequestedAt = DateTime.UtcNow;

        await repository.UpdateAsync(candidate, cancellationToken);
    }

    public async Task<SearchResponse<Candidate>> SearchAsync(
        CandidateSearchRequest searchRequest,
        CancellationToken cancellationToken)
    {
        var candidates = await repository
            .FilterWithChildren<Candidate>(
                x =>
                    // Name
                    (searchRequest.Name == null || x.FirstName.Contains(searchRequest.Name) ||
                     x.LastName.Contains(searchRequest.Name) || x.FullName.Contains(searchRequest.Name)) &&
                    // Candidate Status
                    (searchRequest.CandidateStatus == null || x.CandidateStatus == searchRequest.CandidateStatus) &&
                    // Workflow Status
                    (searchRequest.CandidateWorkflowStepStatus == null ||
                     (x.CandidateWorkflowId != null && x.CandidateWorkflow!.CandidateWorkflowSteps.Any(
                         w =>
                             w.CandidateWorkflowStepStatus == searchRequest.CandidateWorkflowStepStatus))),
                DefaultIncludes)
            .ToArrayAsync(cancellationToken);

        return new(candidates, searchRequest);
    }
}