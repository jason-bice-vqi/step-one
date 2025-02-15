using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Candidates;

public class Candidate : EntityBase<int>, IEntityStatusAssignable
{
    [MaxLength(100)]
    [Required]
    public string FirstName { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string LastName { get; set; }
    
    [Required]
    [StringLength(10, MinimumLength = 10)]
    public string MobilePhoneNumber { get; set; }
    
    /// <summary>
    /// The user's OTP if they're in the process of signing in. Once the user signs in, this is cleared.
    /// </summary>
    [MaybeNull]
    [StringLength(6, MinimumLength = 6)]
    public string OneTimePasscode { get; set; }

    /// <summary>
    /// When the last OTP was created.
    /// </summary>
    public DateTime? OneTimePasswordCreatedAt { get; set; }
    
    /// <summary>
    /// When the last OTP expired.
    /// </summary>
    public DateTime? OneTimePasswordExpiresAt { get; set; }
    
    public int? CandidateWorkflowId { get; set; }
    
    [MaybeNull]
    public CandidateWorkflow CandidateWorkflow { get; set; }

    public EntityStatuses EntityStatus { get; set; }
}