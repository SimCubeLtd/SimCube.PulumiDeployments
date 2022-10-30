using Pulumi.AzureNative.DBforPostgreSQL;
using Pulumi.AzureNative.DBforPostgreSQL.V20220120Preview;
using Pulumi.AzureNative.DBforPostgreSQL.V20220120Preview.Inputs;
using SimCube.PulumiDeployments.Arguments.Azure.PostgreSql.Flexible;
using CreateMode = Pulumi.AzureNative.DBforPostgreSQL.V20220120Preview.CreateMode;
using FirewallRule = Pulumi.AzureNative.DBforPostgreSQL.V20220120Preview.FirewallRule;

namespace SimCube.PulumiDeployments.Resources.Azure.PostgreSql.Flexible;

public sealed class PostgresFlexibleServerResource : BaseAzureResource<PostgresFlexibleServerResource, PostgresFlexibleServerResourceArgs>
{
    public PostgresFlexibleServerResource(string name, PostgresFlexibleServerResourceArgs args, ComponentResourceOptions? options = null) :
        base(name, args, options)
    {
        var serverName = $"{args.ApplicationName}-{ResourceNames.PostgresServer}-{args.Location}-{args.Environment}";
        var vnetRuleName = $"{ResourceNames.PostgresServer}-{ResourceNames.VirtualNetworkRule}-{args.Location}-{args.Environment}";

        Username = Output<string>.Create(Task.FromResult(args.Username ?? $"{args.ApplicationName}-Admin"));

        var randomPasswordName = $"{serverName}-password";
        var adminPassword = RandomPasswordResource.Create(new(randomPasswordName));

        var server = new Pulumi.AzureNative.DBforPostgreSQL.V20220120Preview.Server(
            serverName,
            new()
            {
                ServerName = serverName,
                Location = args.Location,
                AdministratorLogin = Username,
                AdministratorLoginPassword = adminPassword.Result,
                CreateMode = CreateMode.Default.ToString(),
                Storage = new StorageArgs
                {
                    StorageSizeGB = args.ServerStorageGb,
                },
                Backup = new BackupArgs
                {
                    BackupRetentionDays = args.ServerBackupRetentionDays,
                    GeoRedundantBackup = args.ServerGeoRedundantBackup ? GeoRedundantBackupEnum.Enabled : GeoRedundantBackupEnum.Disabled,
                },
                Sku = new SkuArgs
                {
                  Name  = args.ServerFamilyName,
                  Tier = args.ServerFamily,
                },
                Version = args.ServerVersion,
                ResourceGroupName = args.ResourceGroup.ResourceGroupName,
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

        ServerName = Output.Create(serverName);
        AdminPassword = adminPassword.Result;
        ServerFqdn = server.FullyQualifiedDomainName;

        RegisterOutputs();
    }



    public Output<string> GetConnectionString(PostgresFlexibleDatabaseResource databaseResource) =>
        Output.Format(
            $"Host={ServerFqdn};Database={databaseResource.Database.Name};Username={Username};Password={AdminPassword};Trust Server Certificate=True;SSL Mode=Require;");

    public Output<string> Username { get; set; }
    public Output<string> ServerName { get; set;}
    public Output<string> ServerFqdn { get; set;}
    public Output<string>? VNetIntegrationRuleId { get; set;}
    public Output<string>? VNetIntegrationRuleName { get; set;}
    public Output<string> AdminPassword { get; set;}
    public List<Output<string>>? FirewallRuleIds { get; set;}

    private static string GetFirewallRuleName(string serverName, string name) => $"{serverName}-{ResourceNames.FirewallRule}-{name}";
}