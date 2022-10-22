using Pulumi.AzureNative.DBforPostgreSQL;
using Pulumi.AzureNative.DBforPostgreSQL.Inputs;
using SimCube.PulumiDeployments.Arguments.Azure;

namespace SimCube.PulumiDeployments.Resources.Azure;

public sealed class PostgresServerResource : BaseAzureResource<PostgresServerResource, PostgresServerResourceArgs>
{
    public PostgresServerResource(
        string name,
        PostgresServerResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(name, args, options)
    {
        var postgresServerName = $"{args.ApplicationName}-{ResourceNames.PostgresServer}-{args.Location}-{args.Environment}";
        var postgresServerVnetRuleName =
            $"{ResourceNames.PostgresServer}-{ResourceNames.VirtualNetworkRule}-{args.Location}-{args.Environment}";

        Server = new(
            postgresServerName,
            new()
            {
                ServerName = postgresServerName,
                Location = args.Location,
                Properties = new ServerPropertiesForDefaultCreateArgs
                {
                    AdministratorLogin = args.Username ?? string.Empty,
                    AdministratorLoginPassword = args.Password ?? string.Empty,
                    CreateMode = CreateMode.Default.ToString(),
                    MinimalTlsVersion = MinimalTlsVersionEnum.TLS1_2,
                    SslEnforcement = SslEnforcementEnum.Enabled,
                    StorageProfile = new StorageProfileArgs
                    {
                        BackupRetentionDays = args.ServerBackupRetentionDays,
                        GeoRedundantBackup = args.ServerGeoRedundantBackup ? GeoRedundantBackup.Enabled : GeoRedundantBackup.Disabled,
                        StorageMB = args.ServerStorageProfile,
                    },
                    PublicNetworkAccess = PublicNetworkAccessEnum.Enabled,
                    Version = args.ServerVersion,
                },
                ResourceGroupName = args.ResourceGroup.Name,
                Sku = new SkuArgs
                {
                    Capacity = args.ServerCapacity,
                    Family = args.ServerFamily,
                    Name = args.ServerFamilyName,
                    Tier = args.SkuTier,
                },
                Tags = GetResourceTags,
            });

        if (args.SkuTier != SkuTier.Basic)
        {
            VNetIntegrationRule = new(
                postgresServerVnetRuleName,
                new()
                {
                    ServerName = postgresServerName,
                    ResourceGroupName = args.ResourceGroup.Name,
                    VirtualNetworkRuleName = postgresServerVnetRuleName,
                    VirtualNetworkSubnetId = args.VNet is null ? string.Empty : args.VNet.Subnet.Id,
                    IgnoreMissingVnetServiceEndpoint = true,
                },
                new()
                {
                    DependsOn = new()
                    {
                        Server,
                    },
                });
        }

        RegisterOutputs();
    }

    public Server Server { get; }

    public VirtualNetworkRule? VNetIntegrationRule { get; }
}