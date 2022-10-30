using SimCube.PulumiDeployments.Resources.Azure.Storage;

namespace SimCube.PulumiDeployments.Arguments.Azure.Storage;

[ExcludeFromCodeCoverage]
public record QueueStorageResourceArgs : BaseAzureResourceArgs
{
    public string? StorageQueueName { get; set; } = default!;
    public BlobStorageResource BlobStorage { get; init; } = default!;
}