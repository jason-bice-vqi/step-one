﻿using System.ComponentModel.DataAnnotations;
using ViaQuestInc.StepOne.Kernel.Entity;

namespace ViaQuestInc.StepOne.Core.Organization;

public class Company : EntityBase<int>
{
    [MaxLength(50)]
    [Required]
    public required string Name { get; set; }
}