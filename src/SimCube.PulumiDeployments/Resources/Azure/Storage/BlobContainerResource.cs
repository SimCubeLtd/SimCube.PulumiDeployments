using Pulumi.AzureNative.Storage;

namespace SimCube.PulumiDeployments.Resources.Azure.Storage;

public sealed class BlobContainerResource : BaseAzureResource<BlobContainerResource, BlobContainerResourceArgs>
{
    public BlobContainerResource(
        string name,
        BlobContainerResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(
            name,
            args,
            options)
    {
        ArgumentNullException.ThrowIfNull(args.ContainerName, nameof(args.ContainerName));
        ArgumentNullException.ThrowIfNull(args.BlobStorageResource, nameof(args.BlobStorageResource));

        var blobContainer = new BlobContainer(
            args.ContainerName,
            new()
            {
                AccountName = args.BlobStorageResource.StorageAccountName,
                PublicAccess = PublicAccess.None,
                ResourceGroupName = args.ResourceGroup.Name,
                ContainerName = args.ContainerName,
            });

        BlobContainerName = blobContainer.Name;

        RegisterOutputs();
    }

    public Output<string> BlobContainerName { get; }
}