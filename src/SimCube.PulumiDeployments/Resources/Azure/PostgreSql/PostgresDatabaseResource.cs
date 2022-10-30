using Pulumi.AzureNative.DBforPostgreSQL;

namespace SimCube.PulumiDeployments.Resources.Azure.PostgreSql;

public sealed class PostgresDatabaseResource : BaseAzureResource<PostgresDatabaseResource, PostgresDatabaseResourceArgs>
{
    public PostgresDatabaseResource(
        string name,
        PostgresDatabaseResourceArgs args,
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
                ResourceGroupName = args.ResourceGroup.Name,
                ServerName = args.ServerArgs.ServerName,
            },
            new()
            {
                DependsOn = new()
                {
                    args.ServerArgs,
                },
            });

        ConnectionString = GetConnectionString(args);

        RegisterOutputs();
    }

    private Output<string> GetConnectionString(PostgresDatabaseResourceArgs args) =>
        args.ServerArgs.GetConnectionString(this);

    public Database Database { get; }

    public Output<string>? ConnectionString { get; }
}