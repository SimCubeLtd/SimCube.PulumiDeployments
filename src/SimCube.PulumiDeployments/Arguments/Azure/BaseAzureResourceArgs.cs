using Pulumi.AzureNative.Resources;
using SimCube.PulumiDeployments.Configuration.Azure;

namespace SimCube.PulumiDeployments.Arguments.Azure;

public abstract record BaseAzureResourceArgs
{
    public BaseAzureConfiguration Configuration { get; init; } = default!;

    public string Location { get; init; } = default!;

    public ResourceGroup ResourceGroup { get; init; } = default!;
}