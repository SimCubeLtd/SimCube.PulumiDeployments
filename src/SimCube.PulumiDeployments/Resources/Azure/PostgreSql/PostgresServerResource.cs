using Pulumi.AzureNative.DBforPostgreSQL.Inputs;

namespace SimCube.PulumiDeployments.Resources.Azure.PostgreSql;

public sealed class PostgresServerResource : BasePostgresSqlServerResource
{
    public PostgresServerResource(
        string name,
        PostgresServerResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(name, args, options)
    {
    }

    protected override SkuArgs SkuArgs =>
        new()
        {
            Capacity = Args.ServerCapacity,
            Family = Args.ServerFamily,
            Name = Args.ServerFamilyName,
            Tier = Args.SkuTier,
        };


    public override Output<string> GetConnectionString(PostgresDatabaseResource databaseResource) =>
        Output.Format(
            $"Host={ServerFQDN};Database={databaseResource.Database.Name};Username={Username}@{ServerName};Password={AdminPassword};Trust Server Certificate=True;SSL Mode=Require;");
}