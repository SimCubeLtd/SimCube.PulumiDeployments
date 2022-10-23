using Pulumi.AzureNative.DBforPostgreSQL.Inputs;

namespace SimCube.PulumiDeployments.Resources.Azure.PostgreSql;

public sealed class PostgresFlexibleServerResource : BasePostgresSqlServerResource
{
    public PostgresFlexibleServerResource(
        string name,
        PostgresServerResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(name, args, options)
    {
    }

    protected override SkuArgs SkuArgs =>
        new()
        {
            Family = Args.ServerFamily,
            Name = Args.ServerFamilyName,
        };

    public override Output<string> GetConnectionString(PostgresDatabaseResource databaseResource) =>
        Output.Format(
            $"Host={Server?.FullyQualifiedDomainName};Database={databaseResource.Database.Name};Username={Username};Password={AdminPassword?.Result};Trust Server Certificate=True;SSL Mode=Require;");
}