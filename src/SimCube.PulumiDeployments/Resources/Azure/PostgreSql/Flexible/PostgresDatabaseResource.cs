using Pulumi.AzureNative.DBforPostgreSQL.V20220120Preview;
using SimCube.PulumiDeployments.Arguments.Azure.PostgreSql.Flexible;

namespace SimCube.PulumiDeployments.Resources.Azure.PostgreSql.Flexible;

public sealed class PostgresFlexibleDatabaseResource : BaseAzureResource<PostgresFlexibleDatabaseResource, PostgresFlexibleDatabaseResourceArgs>
{
    public PostgresFlexibleDatabaseResource(
        string name,
        PostgresFlexibleDatabaseResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(name, args, options)
    {
        var postgresDatabaseName = args.DatabaseName ?? $"{ResourceNames.PostgresDatabase}-{args.Location}-{args.Environment}";

        Database = new(
            postgresDatabaseName,
            new()
            {
                DatabaseName = postgresDatabaseName,
                Charset = "UTF8",
                Collation = "English_United States.1252",
                ResourceGroupName = args.ResourceGroup.ResourceGroupName,
                ServerName = args.ServerArgs.ServerName,
            }, new()
            {
                DependsOn =
                {
                    args.ServerArgs.FlexibleServer,
                },
            });

        ConnectionString = GetConnectionString(args);

        RegisterOutputs();
    }

    private Output<string> GetConnectionString(PostgresFlexibleDatabaseResourceArgs args) => args.ServerArgs.GetConnectionString(this);

    public Database Database { get; }

    public Output<string>? ConnectionString { get; }
}