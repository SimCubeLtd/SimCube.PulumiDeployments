using Pulumi.AzureNative.Storage;
using SimCube.PulumiDeployments.Resources.Azure.Storage;

namespace SimCube.PulumiDeployments.Arguments.Azure.Storage;

public record BlobResourceArgs : BaseAzureResourceArgs
{
    public string BlobName { get; init; } = $"api-artifacts-{DateTime.UtcNow.ToFileTimeUtc()}.zip";
    public BlobStorageResource Storage { get; init; } = default!;
    public BlobContainerResource Container { get; init; } = default!;
    public BlobType BlobType { get; init; } = BlobType.Block;
    public FileArchive? FileArchive { get; init; } = default!;
}