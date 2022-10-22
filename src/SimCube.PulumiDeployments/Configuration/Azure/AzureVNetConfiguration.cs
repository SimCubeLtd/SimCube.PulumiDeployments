namespace SimCube.PulumiDeployments.Configuration.Azure;

[ExcludeFromCodeCoverage]
public abstract class AzureVNetConfiguration
{
    public string AddressPrefix { get; init; } = default!;
    public bool IncludePublicIp { get; init; }
}