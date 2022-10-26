using Pulumi.AzureNative.Storage;
using SimCube.PulumiDeployments.Helpers.Azure;

namespace SimCube.PulumiDeployments.Resources.Azure.Storage;

public sealed class BlobResource : BaseAzureResource<BlobResource, BlobResourceArgs>
{
    public BlobResource(
        string name,
        BlobResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(
            name,
            args,
            options)
    {
        ArgumentNullException.ThrowIfNull(args.FileArchive, nameof(args.FileArchive));

        var blob = new Blob(
            args.BlobName,
            new()
            {
                AccountName = args.Storage.StorageAccountName,
                ContainerName = args.Container.BlobContainerName,
                ResourceGroupName = args.ResourceGroup.Name,
                Type = args.BlobType,
                Source = args.FileArchive,
            });

        BlobReadUrl = StorageHelpers.GetSignedBlobReadUrl(blob, args.Container.BlobContainerName, args.Storage.StorageAccountName, args.ResourceGroup.Name);

        RegisterOutputs();
    }

    public Output<string>? BlobReadUrl { get; }
}