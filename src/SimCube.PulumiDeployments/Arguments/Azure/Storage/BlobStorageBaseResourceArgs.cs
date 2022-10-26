using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;

namespace SimCube.PulumiDeployments.Arguments.Azure.Storage;

[ExcludeFromCodeCoverage]
public record BlobStorageBaseResourceArgs : BaseAzureResourceArgs
{
    public string? StorageAccountName { get; init; } = default!;

    public EncryptionArgs EncryptionSettings { get; init; } = DefaultEncryptionSettings;

    public KeyPolicyArgs KeyPolicy { get; init; } = new();

    public Kind StorageKind { get; init; } = Kind.StorageV2;

    public RoutingPreferenceArgs RoutingPreference { get; init; } = DefaultRoutingPreferences;

    public NetworkRuleSetArgs NetworkRuleSet { get; init; } = DefaultNetworkRuleSet;

    public SasPolicyArgs SasPolicy { get; init; } = DefaultSasPolicy;

    public SkuArgs Sku { get; init; } = new()
    {
        Name = SkuName.Standard_LRS,
    };

    private static SasPolicyArgs DefaultSasPolicy => new()
    {
        ExpirationAction = ExpirationAction.Log,
        SasExpirationPeriod = "1.15:59:59",
    };

    private static NetworkRuleSetArgs DefaultNetworkRuleSet => new()
    {
        Bypass = Bypass.AzureServices,
        DefaultAction = DefaultAction.Allow,
    };

    private static RoutingPreferenceArgs DefaultRoutingPreferences => new()
    {
        PublishInternetEndpoints = false,
        PublishMicrosoftEndpoints = true,
        RoutingChoice = RoutingChoice.MicrosoftRouting,
    };

    private static EncryptionArgs DefaultEncryptionSettings => new()
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
    };
}