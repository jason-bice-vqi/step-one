﻿using System.ComponentModel.DataAnnotations;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Candidates;

public class Candidate : EntityBase<int>, IEntityStatusAssignable
{
    [MaxLength(100)]
    [Required]
    public required string FirstName { get; set; }
    
    [MaxLength(100)]
    [Required]
    public required string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";
    
    [Required]
    [StringLength(10, MinimumLength = 10)]
    public required string MobilePhoneNumber { get; set; }
    
    /// <summary>
    /// The user's OTP if they're in the process of signing in. Once the user signs in, this is cleared.
    /// </summary>
    [StringLength(6, MinimumLength = 6)]
    public string? OneTimePasscode { get; set; }

    /// <summary>
    /// When the last OTP was created.
    /// </summary>
    public DateTime? OneTimePasswordCreatedAt { get; set; }
    
    /// <summary>
    /// When the last OTP expired.
    /// </summary>
    public DateTime? OneTimePasswordExpiresAt { get; set; }
    
    /// <summary>
    /// When the candidate was last authenticated (last logon).
    /// </summary>
    public DateTime? LastAuthenticatedAt { get; set; }
    
    public int? CandidateWorkflowId { get; set; }
    
    /// <summary>
    /// The workflow associated with this candidate.
    /// </summary>
    public CandidateWorkflow? CandidateWorkflow { get; set; }

    public required EntityStatuses EntityStatus { get; set; }
}