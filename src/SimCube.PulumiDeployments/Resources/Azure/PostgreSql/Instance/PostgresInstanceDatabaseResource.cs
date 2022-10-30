using Pulumi.AzureNative.DBforPostgreSQL;

namespace SimCube.PulumiDeployments.Resources.Azure.PostgreSql.Instance;

public sealed class PostgresInstanceDatabaseResource : BaseAzureResource<PostgresInstanceDatabaseResource, PostgresInstanceDatabaseResourceArgs>
{
    public PostgresInstanceDatabaseResource(
        string name,
        PostgresInstanceDatabaseResourceArgs args,
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
                    args.ServerArgs.InstanceServer,
                },
            });

        ConnectionString = GetConnectionString(args);

        RegisterOutputs();
    }

    private Output<string> GetConnectionString(PostgresInstanceDatabaseResourceArgs args) => args.ServerArgs.GetConnectionString(this);

    public Database Database { get; }

    public Output<string>? ConnectionString { get; }
}