namespace SimCube.PulumiDeployments.Resources.Azure.PostgreSql;

public abstract class BasePostgresServerInstance : BaseAzureResource<BasePostgresServerInstance, PostgresServerResourceArgs>
{
    protected BasePostgresServerInstance(string name, PostgresServerResourceArgs args, ComponentResourceOptions? options = null) :
        base(name, args, options)
    {
    }

    public Output<string> Username { get; set; } = default!;
    public Output<string> ServerName { get; set;} = default!;
    public Output<string> ServerFqdn { get; set;} = default!;
    public Output<string>? VNetIntegrationRuleId { get; set;} = default!;
    public Output<string>? VNetIntegrationRuleName { get; set;} = default!;
    public Output<string> AdminPassword { get; set;} = default!;
    public List<Output<string>>? FirewallRuleIds { get; set;} = default!;

    public abstract Output<string> GetConnectionString(PostgresDatabaseResource databaseResource);
}