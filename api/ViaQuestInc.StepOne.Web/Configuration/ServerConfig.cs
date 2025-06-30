using System.ComponentModel.DataAnnotations;

namespace ViaQuestInc.StepOne.Web.Configuration;

public class ServerConfig
{
    [Required]
    public string PathBase { get; set; }
}