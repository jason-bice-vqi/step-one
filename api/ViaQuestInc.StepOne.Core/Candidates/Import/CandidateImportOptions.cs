using System.Data;

namespace ViaQuestInc.StepOne.Core.Candidates.Import;

public class CandidateImportOptions
{
    /// <summary>
    /// The raw data record that's currently being processed.
    /// </summary>
    public DataRow? CurrentRawCandidateDataRow { get; set; }

    public Candidate? InitializedCandidateEntity { get; set; }

    /// <summary>
    /// Indicates whether processing of the current record/entity/candidate should be aborted. 
    /// </summary>
    public bool Abort { get; set; }
}