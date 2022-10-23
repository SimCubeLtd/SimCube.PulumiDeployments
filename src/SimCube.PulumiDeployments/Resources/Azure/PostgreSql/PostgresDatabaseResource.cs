using Pulumi.AzureNative.DBforPostgreSQL;
using SimCube.PulumiDeployments.Arguments.Azure;

namespace SimCube.PulumiDeployments.Resources.Azure.PostgreSql;

public sealed class PostgresDatabaseResource : BaseAzureResource<PostgresDatabaseResource, PostgresDatabaseResourceArgs>
{
    public PostgresDatabaseResource(
        string name,
        PostgresDatabaseResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(name, args, options)
    {
        ArgumentNullException.ThrowIfNull(args.ServerArgs.Server, nameof(args.ServerArgs.Server));

        var postgresDatabaseName = args.DatabaseName ?? $"{ResourceNames.PostgresDatabase}-{args.Location}-{args.Environment}";

        Database = new(
            postgresDatabaseName,
            new()
            {
                DatabaseName = postgresDatabaseName,
                Charset = "UTF8",
                Collation = "English_United States.1252",
                ResourceGroupName = args.ResourceGroup.Name,
                ServerName = args.ServerArgs.Server.Name,
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