namespace SimCube.PulumiDeployments.Configuration.Azure;

public abstract class BaseAzureConfiguration
{
    public string SupportAddress { get; set; } = default!;
    public string ApplicationName { get; set; } = default!;
    public string Environment { get; set; } = default!;
    public AzureVNetConfiguration? VirtualNetwork { get; set; } = default!;
    public AzurePostgresConfiguration? AzurePostgresConfiguration { get; set; } = default!;
}