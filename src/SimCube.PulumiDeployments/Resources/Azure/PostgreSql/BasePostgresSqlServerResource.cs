using Pulumi.AzureNative.DBforPostgreSQL;
using Pulumi.AzureNative.DBforPostgreSQL.Inputs;

namespace SimCube.PulumiDeployments.Resources.Azure.PostgreSql;

public abstract class BasePostgresSqlServerResource : BaseAzureResource<BasePostgresSqlServerResource, PostgresServerResourceArgs>
{
    protected BasePostgresSqlServerResource(string name, PostgresServerResourceArgs args, ComponentResourceOptions? options = null) :
        base(name, args, options)
    {
        Args = args;
        var serverName = $"{args.ApplicationName}-{ResourceNames.PostgresServer}-{args.Location}-{args.Environment}";
        var vnetRuleName = $"{ResourceNames.PostgresServer}-{ResourceNames.VirtualNetworkRule}-{args.Location}-{args.Environment}";
        Username = args.Username ?? $"{args.ApplicationName}-Admin";

        var randomPasswordName = $"{serverName}-password";
        AdminPassword = RandomPasswordResource.Create(new(randomPasswordName));

        Server = new(
            serverName,
            new()
            {
                ServerName = serverName,
                Location = args.Location,
                Properties = new ServerPropertiesForDefaultCreateArgs
                {
                    AdministratorLogin = Username,
                    AdministratorLoginPassword = AdminPassword.Result,
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
                Sku = SkuArgs,
                Tags = GetResourceTags,
            });

        if (args.VNet != null)
        {
            VNetIntegrationRule = new(
                vnetRuleName,
                new()
                {
                    ServerName = serverName,
                    ResourceGroupName = args.ResourceGroup.Name,
                    VirtualNetworkRuleName = vnetRuleName,
                    VirtualNetworkSubnetId = args.VNet!.Subnet.Id,
                    IgnoreMissingVnetServiceEndpoint = true,
                },
                new() {DependsOn = new() {Server!,},});
        }

        if (args.ShouldCreateFirewallRules && args.FirewallAllowedIpAddresses.Count > 0)
        {
            FirewallRules = new();

            foreach (var allowedIpAddress in args.FirewallAllowedIpAddresses)
            {
                var ruleName = GetFirewallRuleName(serverName, allowedIpAddress.Key);

                FirewallRules.Add(
                    new(
                        ruleName,
                        new()
                        {
                            ResourceGroupName = args.ResourceGroup.Name,
                            ServerName = serverName,
                            StartIpAddress = allowedIpAddress.Value,
                            EndIpAddress = allowedIpAddress.Value,
                            FirewallRuleName = allowedIpAddress.Key,
                        }));
            }
        }

        RegisterOutputs();
    }

    public string Username { get; }
    public Server? Server { get; }
    public VirtualNetworkRule? VNetIntegrationRule { get; }
    public RandomPasswordResource? AdminPassword { get; }
    public List<FirewallRule>? FirewallRules { get; }

    protected readonly PostgresServerResourceArgs Args;
    protected abstract SkuArgs SkuArgs { get; }

    public abstract Output<string> GetConnectionString(PostgresDatabaseResource databaseResource);

    private static string GetFirewallRuleName(string serverName, string name) => $"{serverName}-{ResourceNames.FirewallRule}-{name}";
}