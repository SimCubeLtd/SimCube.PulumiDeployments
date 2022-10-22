using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using SimCube.PulumiDeployments.Arguments.Azure;

namespace SimCube.PulumiDeployments.Resources.Azure;

public sealed class BasicStorageResource : BaseAzureResource<StorageResource, BasicStorageBaseResourceArgs>
{
    public BasicStorageResource(
        string name,
        BasicStorageBaseResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(
            name,
            args,
            options)
    {
        StorageAccount = new(
            args.StorageAccountName,
            new()
            {
                AccountName = args.StorageAccountName,
                ResourceGroupName = args.ResourceGroup.Name,
                Sku = new SkuArgs
                {
                    Name = SkuName.Standard_LRS,
                },
                Kind = Kind.StorageV2,
                Tags = GetResourceTags,
            });

        BlobContainer = new(
            args.DeploymentZipContainerName,
            new()
            {
                AccountName = StorageAccount.Name,
                PublicAccess = PublicAccess.None,
                ResourceGroupName = args.ResourceGroup.Name,
                ContainerName = args.DeploymentZipContainerName,
            });

        RegisterOutputs();
    }

    public StorageAccount StorageAccount { get; }

    public BlobContainer BlobContainer { get; }
}