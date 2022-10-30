using Pulumi.AzureNative.Storage;

namespace SimCube.PulumiDeployments.Resources.Azure.Storage;

public sealed class BlobStorageResource : BaseAzureResource<BlobStorageResource, BlobStorageBaseResourceArgs>
{
    public BlobStorageResource(
        string name,
        BlobStorageBaseResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(
            name,
            args,
            options)
    {
        var defaultStorageAccountName = $"{ResourceNames.StorageAccount}-{args.Location}-{args.Environment}";

        var storageAccount = new StorageAccount(
            args.StorageAccountName ?? defaultStorageAccountName,
            new()
            {
                AccountName = args.StorageAccountName ?? defaultStorageAccountName,
                AllowBlobPublicAccess = false,
                AllowSharedKeyAccess = true,
                Encryption = args.EncryptionSettings,
                IsHnsEnabled = true,
                KeyPolicy = args.KeyPolicy,
                Kind = args.StorageKind,
                Location = args.Location,
                MinimumTlsVersion = MinimumTlsVersion.TLS1_2,
                ResourceGroupName = args.ResourceGroup.Name,
                RoutingPreference = args.RoutingPreference,
                NetworkRuleSet = args.NetworkRuleSet,
                SasPolicy = args.SasPolicy,
                Sku = args.Sku,
                Tags = GetResourceTags,
            });

        StorageAccountName = storageAccount.Name;

        RegisterOutputs();
    }

    public Output<string> StorageAccountName { get; }
}