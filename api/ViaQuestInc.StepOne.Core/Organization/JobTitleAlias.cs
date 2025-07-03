using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ViaQuestInc.StepOne.Core.Data.Entity;

namespace ViaQuestInc.StepOne.Core.Organization;

/// <summary>
/// Establishes a relationship between a formal/proper job title known to a company at ViaQuest, and a free-form title
/// as listed in the Paycor ATS, Indeed job listing, etc.
/// </summary>
[Index(nameof(Alias), IsUnique = true)]
public class JobTitleAlias : EntityBase<int>
{
    public required int JobTitleId { get; set; }

    public JobTitle JobTitle { get; set; }

    /// <summary>
    /// The job title name as it appears in the ATS.
    /// </summary>
    [MaxLength(255)]
    [Required]
    public required string Alias { get; set; }
}