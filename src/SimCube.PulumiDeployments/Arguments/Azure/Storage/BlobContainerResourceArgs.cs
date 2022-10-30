using SimCube.PulumiDeployments.Resources.Azure.Storage;

namespace SimCube.PulumiDeployments.Arguments.Azure.Storage;

public record BlobContainerResourceArgs : BaseAzureResourceArgs
{
    public string? ContainerName { get; init; } = default!;
    public BlobStorageResource BlobStorageResource { get; init; } = default!;
}