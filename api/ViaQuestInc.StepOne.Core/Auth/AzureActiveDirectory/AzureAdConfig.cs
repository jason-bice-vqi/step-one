﻿namespace ViaQuestInc.StepOne.Core.Auth.AzureActiveDirectory;

public class AzureAdConfig
{
    public string ClientId { get; set; }
    
    public string ClientSecret { get; set; }

    public string Domain { get; set; }

    public string Instance { get; set; }
    
    public string TenantId { get; set; }
}