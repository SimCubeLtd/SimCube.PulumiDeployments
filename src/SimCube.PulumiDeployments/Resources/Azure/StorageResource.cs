using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using SimCube.PulumiDeployments.Arguments.Azure;

namespace SimCube.PulumiDeployments.Resources.Azure;

public sealed class StorageResource : BaseAzureResource<StorageResource, StorageResourceArgs>
{
    private const string ZipContainerName = "zip-container";
    private const string MessageContainerName = "message-container";

    public StorageResource(
        string name,
        StorageResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(
            name,
            args,
            options)
    {
        var storageAccountName = $"{ResourceNames.StorageAccount}-{args.Location}-{args.Environment}";
        var storageQueueName = $"{ResourceNames.StorageQueue}-{args.Location}-{args.Environment}";

        var storageAccount = new StorageAccount(
            storageAccountName,
            new()
            {
                AccountName = storageAccountName,
                AllowBlobPublicAccess = false,
                AllowSharedKeyAccess = true,
                Encryption =
                    new EncryptionArgs
                    {
                        KeySource = KeySource.Microsoft_Storage,
                        RequireInfrastructureEncryption = false,
                        Services = new EncryptionServicesArgs
                        {
                            Blob = new EncryptionServiceArgs
                            {
                                Enabled = true,
                                KeyType = KeyType.Account,
                            },
                            File = new EncryptionServiceArgs
                            {
                                Enabled = true,
                                KeyType = KeyType.Account,
                            },
                            Queue = new EncryptionServiceArgs
                            {
                                Enabled = true,
                                KeyType = KeyType.Account,
                            },
                        },
                    },
                IsHnsEnabled = true,
                KeyPolicy = new KeyPolicyArgs
                {
                    KeyExpirationPeriodInDays = 30,
                },
                Kind = Kind.StorageV2,
                Location = args.Location,
                MinimumTlsVersion = MinimumTlsVersion.TLS1_2,
                ResourceGroupName = args.ResourceGroup.Name,
                RoutingPreference =
                    new RoutingPreferenceArgs
                    {
                        PublishInternetEndpoints = false,
                        PublishMicrosoftEndpoints = true,
                        RoutingChoice = RoutingChoice.MicrosoftRouting,
                    },
                NetworkRuleSet = new NetworkRuleSetArgs
                {
                    Bypass = Bypass.AzureServices,
                    DefaultAction = DefaultAction.Allow,
                    VirtualNetworkRules =
                    {
                        new VirtualNetworkRuleArgs
                        {
                            VirtualNetworkResourceId = args.VNet is null ? string.Empty : args.VNet.Subnet.Id,
                        },
                    },
                },
                SasPolicy = new SasPolicyArgs
                {
                    ExpirationAction = ExpirationAction.Log,
                    SasExpirationPeriod = "1.15:59:59",
                },
                Sku = new SkuArgs
                {
                    Name = SkuName.Standard_LRS,
                },
                Tags = GetResourceTags,
            });

        var storageQueue = new Queue(
            storageQueueName,
            new()
            {
                AccountName = storageAccountName,
                QueueName = storageQueueName,
                ResourceGroupName = args.ResourceGroup.Name,
            });

        var zipsContainer = new BlobContainer(
            ZipContainerName,
            new()
            {
                ContainerName = ZipContainerName,
                AccountName = storageAccountName,
                PublicAccess = PublicAccess.None,
                ResourceGroupName = args.ResourceGroup.Name,
            });

        var messageContainer = new BlobContainer(
            ZipContainerName,
            new()
            {
                ContainerName = MessageContainerName,
                AccountName = storageAccountName,
                PublicAccess = PublicAccess.None,
                ResourceGroupName = args.ResourceGroup.Name,
            });

        StorageAccountName = storageAccount.Name;
        StorageAccountId = storageAccount.Id;
        StorageQueueId = storageQueue.Id;
        ZipsContainerId = zipsContainer.Id;
        ZipsContainerName = zipsContainer.Name;
        MessagesContainerId = zipsContainer.Id;
        MessagesContainerName = zipsContainer.Name;

        RegisterOutputs();
    }

    public Output<string> StorageAccountId { get; }

    public Output<string> StorageAccountName { get; }

    public Output<string> StorageQueueId { get; }

    public Output<string> ZipsContainerId { get; }

    public Output<string> ZipsContainerName { get; }

    public Output<string> MessagesContainerId { get; }

    public Output<string> MessagesContainerName { get; }
}