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
                ResourceGroupName = args.ResourceGroup.ResourceGroupName,
                RoutingPreference = args.RoutingPreference,
                NetworkRuleSet = args.NetworkRuleSet,
                SasPolicy = args.SasPolicy,
                Sku = args.Sku,
                Tags = GetResourceTags,
            });

        StorageAccountName = storageAccount.Name;
        StorageAccountConnectionString = GetConnectionString(args.ResourceGroup.ResourceGroupName, StorageAccountName);

        RegisterOutputs();
    }
    
    public Output<string> StorageAccountName { get; }
    public Output<string> StorageAccountConnectionString { get; }
    
    public static Output<string> GetConnectionString(Input<string> resourceGroupName, Input<string> accountName)
    {
        var storageAccountKeys = ListStorageAccountKeys.Invoke(new()
        {
            ResourceGroupName = resourceGroupName,
            AccountName = accountName,
        });

        return storageAccountKeys.Apply(keys =>
        {
            var primaryStorageKey = keys.Keys[0].Value;

            return Output.Format($"DefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={primaryStorageKey}");
        });
    }
}