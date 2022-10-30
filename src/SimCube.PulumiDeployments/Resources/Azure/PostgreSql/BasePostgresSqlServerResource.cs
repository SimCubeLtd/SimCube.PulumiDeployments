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
        Username = Output<string>.Create(Task.FromResult(args.Username ?? $"{args.ApplicationName}-Admin"));

        var randomPasswordName = $"{serverName}-password";
        var adminPassword = RandomPasswordResource.Create(new(randomPasswordName));

        var server = new Server(
            serverName,
            new()
            {
                ServerName = serverName,
                Location = args.Location,
                Properties = new ServerPropertiesForDefaultCreateArgs
                {
                    AdministratorLogin = Username,
                    AdministratorLoginPassword = adminPassword.Result,
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
                ResourceGroupName = args.ResourceGroup.ResourceGroupName,
                Sku = SkuArgs,
                Tags = GetResourceTags,
            });

        if (args.VNet != null)
        {
            var vNetIntegrationRule = new VirtualNetworkRule(
                vnetRuleName,
                new()
                {
                    ServerName = serverName,
                    ResourceGroupName = args.ResourceGroup.ResourceGroupName,
                    VirtualNetworkRuleName = vnetRuleName,
                    VirtualNetworkSubnetId = args.VNet!.SubnetId,
                    IgnoreMissingVnetServiceEndpoint = true,
                },
                new() {DependsOn = new() {server,},});

            VNetIntegrationRuleId = vNetIntegrationRule.Id;
            VNetIntegrationRuleName = vNetIntegrationRule.Name;
        }

        if (args.ShouldCreateFirewallRules && args.FirewallAllowedIpAddresses.Count > 0)
        {
            var firewallRules = new List<FirewallRule>();

            foreach (var allowedIpAddress in args.FirewallAllowedIpAddresses)
            {
                var ruleName = GetFirewallRuleName(serverName, allowedIpAddress.Key);

                firewallRules.Add(
                    new(
                        ruleName,
                        new()
                        {
                            ResourceGroupName = args.ResourceGroup.ResourceGroupName,
                            ServerName = serverName,
                            StartIpAddress = allowedIpAddress.Value,
                            EndIpAddress = allowedIpAddress.Value,
                            FirewallRuleName = allowedIpAddress.Key,
                        }));
            }

            FirewallRuleIds = firewallRules.Select(x => x.Id).ToList();
        }

        ServerName = server.Name;
        AdminPassword = adminPassword.Result;
        ServerFQDN = server.FullyQualifiedDomainName;

        RegisterOutputs();
    }

    public Output<string> Username { get; }
    public Output<string> ServerName { get; }
    public Output<string?> ServerFQDN { get; }
    public Output<string>? VNetIntegrationRuleId { get; }
    public Output<string>? VNetIntegrationRuleName { get; }
    public Output<string> AdminPassword { get; }
    public List<Output<string>>? FirewallRuleIds { get; }

    protected readonly PostgresServerResourceArgs Args;
    protected abstract SkuArgs SkuArgs { get; }

    public abstract Output<string> GetConnectionString(PostgresDatabaseResource databaseResource);

    private static string GetFirewallRuleName(string serverName, string name) => $"{serverName}-{ResourceNames.FirewallRule}-{name}";
}