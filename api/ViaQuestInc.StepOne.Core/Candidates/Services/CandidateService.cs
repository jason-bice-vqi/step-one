using System.Data;
using ViaQuestInc.StepOne.Core.Candidates.Import;
using ViaQuestInc.StepOne.Infrastructure.Services;
using ViaQuestInc.StepOne.Kernel.Data;

namespace ViaQuestInc.StepOne.Core.Candidates.Services;

public class CandidateService(
    CandidateImportEngine candidateImportEngine,
    ExcelService excelService,
    IRepository repository)
{
    private const int HeaderRow = 7;
    
    public async Task<Candidate?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        return await repository.GetAsync<Candidate>(x => x.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<ICollection<Candidate>> ImportAsync(MemoryStream candidatesMemoryStream,
        CancellationToken cancellationToken)
    {
        using var dataSet = await excelService.ToDataSetAsync(candidatesMemoryStream, HeaderRow, cancellationToken);

        var rawCandidateDataRows = dataSet.Tables[0].AsEnumerable();

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
}