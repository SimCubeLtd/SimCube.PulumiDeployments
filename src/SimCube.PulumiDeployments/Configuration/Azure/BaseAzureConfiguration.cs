namespace SimCube.PulumiDeployments.Configuration.Azure;

public abstract class BaseAzureConfiguration
{
    protected readonly Config Config = new();

    public string SupportAddress { get; set; } = default!;
    public string ApplicationName { get; set; } = default!;
    public string Environment { get; set; } = default!;
    public AzureVNetConfiguration VirtualNetwork => Config.GetFromJson<AzureVNetConfiguration>(nameof(AzureVNetConfiguration));
}