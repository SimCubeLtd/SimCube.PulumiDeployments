using Pulumi.AzureNative.DBforPostgreSQL;

namespace SimCube.PulumiDeployments.Resources.Azure.PostgreSql;

public abstract class BasePostgresSqlServerResource : BaseAzureResource<BasePostgresSqlServerResource, PostgresServerResourceArgs>
{
    protected BasePostgresSqlServerResource(string name, PostgresServerResourceArgs args, ComponentResourceOptions? options = null) :
        base(name, args, options)
    {
        ServerName = $"{args.ApplicationName}-{ResourceNames.PostgresServer}-{args.Location}-{args.Environment}";
        _vnetRuleName = $"{ResourceNames.PostgresServer}-{ResourceNames.VirtualNetworkRule}-{args.Location}-{args.Environment}";
        Username = args.Username ?? $"{args.ApplicationName}-Admin";

        var randomPasswordName = $"{ServerName}-password";
        AdminPassword = RandomPasswordResource.Create(new(randomPasswordName));
    }

    protected void Initialise(PostgresServerResourceArgs args)
    {
        CreateServer(args);
        CreateVNetIntegrationRule(args);
        CreateFirewallRules(args);
    }

    private readonly string _vnetRuleName;

    protected readonly string ServerName;

    public string Username { get; }
    public Server? Server { get; init; }

    public VirtualNetworkRule? VNetIntegrationRule { get; private set; }
    public RandomPasswordResource? AdminPassword { get; }
    public List<FirewallRule>? FirewallRules { get; private set; }

    public abstract Output<string> GetConnectionString(PostgresDatabaseResource databaseResource);

    protected abstract Server CreateServer(PostgresServerResourceArgs args);

    private void CreateVNetIntegrationRule(PostgresServerResourceArgs args)
    {
        if (args.VNet == null)
        {
            return;
        }

        VNetIntegrationRule = new(
            _vnetRuleName,
            new()
            {
                ServerName = ServerName,
                ResourceGroupName = args.ResourceGroup.Name,
                VirtualNetworkRuleName = _vnetRuleName,
                VirtualNetworkSubnetId = args.VNet!.Subnet.Id,
                IgnoreMissingVnetServiceEndpoint = true,
            },
            new() {DependsOn = new() {Server!,},});
    }

    private void CreateFirewallRules(PostgresServerResourceArgs args)
    {
        if (!args.FirewallAllowedIpAddresses.Any() || !args.FirewallAllowedIpAddresses.Any())
        {
            return;
        }

        FirewallRules = new List<FirewallRule>();

        foreach (var allowedIpAddress in args.FirewallAllowedIpAddresses)
        {
            var name = GetFirewallRuleName(allowedIpAddress.Key);

            FirewallRules.Add(
                new(
                    name,
                    new()
                    {
                        ResourceGroupName = args.ResourceGroup.Name,
                        ServerName = ServerName,
                        StartIpAddress = allowedIpAddress.Value,
                        EndIpAddress = allowedIpAddress.Value,
                        FirewallRuleName = allowedIpAddress.Key,
                    }));
        }
    }

    private string GetFirewallRuleName(string name) => $"{ServerName}-{ResourceNames.FirewallRule}-{name}";
}