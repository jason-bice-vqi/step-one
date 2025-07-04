﻿using System.ComponentModel.DataAnnotations;
using ViaQuestInc.StepOne.Core.Data.Entity;

namespace ViaQuestInc.StepOne.Core.Organization;

public class Company : EntityBase<int>,
    IHrtDerivedEntity
{
    [MaxLength(50)]
    [Required]
    public required string Name { get; set; }

    [MaxLength(15)]
    [Required]
    public required string Abbreviation { get; set; }

    public required Guid HrtId { get; set; }

    public required EntityStatuses EntityStatus { get; set; }
}