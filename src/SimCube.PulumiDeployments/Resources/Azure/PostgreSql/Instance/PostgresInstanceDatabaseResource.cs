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
        DatabaseName = args.DatabaseName ?? $"{ResourceNames.PostgresDatabase}-{args.Location}-{args.Environment}";

        _ = new Database(
            DatabaseName,
            new()
            {
                DatabaseName = DatabaseName,
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

        ConnectionString = args.ServerArgs.GetConnectionString(this);

        RegisterOutputs();
    }

    public string DatabaseName { get; }

    public Output<string>? ConnectionString { get; }
}