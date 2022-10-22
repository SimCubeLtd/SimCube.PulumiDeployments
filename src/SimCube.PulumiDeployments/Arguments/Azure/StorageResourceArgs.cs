using SimCube.PulumiDeployments.Resources.Azure;

namespace SimCube.PulumiDeployments.Arguments.Azure;

[ExcludeFromCodeCoverage]
public record StorageResourceArgs : BaseAzureResourceArgs
{
    public VirtualNetworkResource VNet { get; init; } = default!;
}