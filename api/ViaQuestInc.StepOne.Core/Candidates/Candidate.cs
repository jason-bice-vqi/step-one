using System.ComponentModel.DataAnnotations;
using Humanizer;
using ViaQuestInc.StepOne.Core.Candidates.Workflows;
using ViaQuestInc.StepOne.Core.Data.Entity;
using ViaQuestInc.StepOne.Core.Organization;

namespace ViaQuestInc.StepOne.Core.Candidates;

public class Candidate
    : EntityBase<int>,
        IEntityStatusAssignable
{
    private const int PaycorCandidateIdLength = 32;
    private const int PhoneLength = 10;

    [MaxLength(100)]
    [Required]
    public required string FirstName { get; set; }

    [MaxLength(100)]
    [Required]
    public required string LastName { get; set; }

    [MaxLength(255)]
    [Required]
    public required string FullName { get; set; }

    [MaxLength(255)]
    public string? Email { get; set; }

    [MaxLength(255)]
    public string? AddressLine1 { get; set; }

    [MaxLength(255)]
    public string? AddressLine2 { get; set; }

    [MaxLength(255)]
    public string? City { get; set; }

    [StringLength(50, MinimumLength = 2)]
    public string? State { get; set; }

    [StringLength(10, MinimumLength = 5)]
    public string? PostalCode { get; set; }

    public required DateTime ImportedAt { get; set; }

    public required DateOnly HireDate { get; set; }

    public DateOnly? StartDate { get; set; }

    [Required]
    [StringLength(PhoneLength, MinimumLength = PhoneLength)]
    public required string PhoneNumber { get; set; }

    [Required]
    [StringLength(PaycorCandidateIdLength, MinimumLength = PaycorCandidateIdLength)]
    public required string PaycorCandidateId { get; set; }

    [MaxLength(255)]
    [Required]
    public required string AtsJobTitle { get; set; }

    public required int AtsJobId { get; set; }

    public int? JobTitleId { get; set; }

    public JobTitle? JobTitle { get; set; }

    /// <summary>
    /// When the last OTP was requested.
    /// </summary>
    public DateTime? OtpLastRequestedAt { get; set; }

    /// <summary>
    /// When the candidate was last authenticated (last logon).
    /// </summary>
    public DateTime? LastAuthenticatedAt { get; set; }

    public int? CandidateWorkflowId { get; set; }

    /// <summary>
    /// The workflow associated with this candidate.
    /// </summary>
    public CandidateWorkflow? CandidateWorkflow { get; set; }

    public required CandidateWorkflowStatus CandidateWorkflowStatus { get; set; }

    public string CandidateWorkflowStatusDesc => CandidateWorkflowStatus.Humanize();

    public required EntityStatuses EntityStatus { get; set; }
}