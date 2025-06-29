using System.ComponentModel.DataAnnotations;
using ViaQuestInc.StepOne.Core.Data.Entity;

namespace ViaQuestInc.StepOne.Core.Organization;

public class Region : EntityBase<int>
{
    [MaxLength(50)] [Required] public required string Name { get; set; }
}