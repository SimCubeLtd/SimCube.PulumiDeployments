using Pulumi.AzureNative.DBforPostgreSQL;
using Pulumi.AzureNative.DBforPostgreSQL.Inputs;
using InstanceServer = Pulumi.AzureNative.DBforPostgreSQL.Server;

namespace SimCube.PulumiDeployments.Resources.Azure.PostgreSql.Instance;

public sealed class PostgresInstanceServerResource : BaseAzureResource<PostgresInstanceServerResource, PostgresInstanceServerResourceArgs>
{
    public PostgresInstanceServerResource(string name, PostgresInstanceServerResourceArgs args, ComponentResourceOptions? options = null) :
        base(name, args, options)
    {
        var serverName = $"{args.ApplicationName}-{ResourceNames.PostgresServer}-{args.Location}-{args.Environment}";
        var vnetRuleName = $"{ResourceNames.PostgresServer}-{ResourceNames.VirtualNetworkRule}-{args.Location}-{args.Environment}";
        Username = Output<string>.Create(Task.FromResult(args.Username ?? $"{args.ApplicationName}-Admin"));

        var randomPasswordName = $"{serverName}-password";
        var adminPassword = RandomPasswordResource.Create(new(randomPasswordName));

        InstanceServer = new(
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
                        StorageMB = args.ServerStorageMb,
                    },
                    PublicNetworkAccess = PublicNetworkAccessEnum.Enabled,
                    Version = args.ServerVersion,
                },
                ResourceGroupName = args.ResourceGroup.ResourceGroupName,
                Sku = new SkuArgs
                {
                    Capacity = args.ServerCapacity,
                    Family = args.ServerFamily,
                    Name = args.ServerFamilyName,
                    Tier = args.SkuTier,
                },
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
                new() {DependsOn = new() {InstanceServer,},});

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

        ServerName = InstanceServer.Name;
        AdminPassword = adminPassword.Result;
        ServerFqdn = SetDomainName(InstanceServer);

        RegisterOutputs();
    }

    public Output<string> GetConnectionString(PostgresInstanceDatabaseResource databaseResource) =>
        Output.Format(
            $"Host={ServerFqdn};Database={databaseResource.Database.Name};Username={Username}@{ServerName};Password={AdminPassword};Trust Server Certificate=True;SSL Mode=Require;");

    public InstanceServer InstanceServer { get; }

    public Output<string> Username { get; set; }
    public Output<string> ServerName { get; set;}
    public Output<string> ServerFqdn { get; set;}
    public Output<string>? VNetIntegrationRuleId { get; set;}
    public Output<string>? VNetIntegrationRuleName { get; set;}
    public Output<string> AdminPassword { get; set;}
    public List<Output<string>>? FirewallRuleIds { get; set;}

    private static string GetFirewallRuleName(string serverName, string name) => $"{serverName}-{ResourceNames.FirewallRule}-{name}";

    private static Output<string> SetDomainName(Server server) => server.FullyQualifiedDomainName.Apply(x => x ?? string.Empty);
}