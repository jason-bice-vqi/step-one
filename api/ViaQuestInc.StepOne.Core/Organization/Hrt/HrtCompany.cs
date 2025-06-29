using System.ComponentModel.DataAnnotations.Schema;
using ViaQuestInc.StepOne.Core.Data.Entity;

namespace ViaQuestInc.StepOne.Core.Organization.Hrt;

public class HrtCompany : EntityBase<Guid>
{
    [Column("CompanyID")]
    public override Guid Id { get; set; }
    
    [Column("CompanyName")]
    public string Name { get; set; }
    
    public string Abbreviation { get; set; }
    
    [Column("Status")]
    public EntityStatuses EntityStatus { get; set; }
}