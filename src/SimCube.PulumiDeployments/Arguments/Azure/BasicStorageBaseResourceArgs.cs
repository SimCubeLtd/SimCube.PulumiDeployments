using SimCube.PulumiDeployments.Resources.Azure;

namespace SimCube.PulumiDeployments.Arguments.Azure;

[ExcludeFromCodeCoverage]
public record BasicStorageBaseResourceArgs : BaseAzureResourceArgs
{
    public string StorageAccountName { get; init; } = default!;
    public string DeploymentZipContainerName { get; init; } = "deploymentzips";
    public VirtualNetworkResource VNet { get; init; } = default!;
}