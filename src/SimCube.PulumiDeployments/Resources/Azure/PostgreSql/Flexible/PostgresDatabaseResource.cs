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
        DatabaseName = args.DatabaseName ?? $"{ResourceNames.PostgresDatabase}-{args.Location}-{args.Environment}";

        _ = new Database(
            DatabaseName,
            new()
            {
                DatabaseName = DatabaseName,
                Charset = args.CharSet,
                Collation = args.Collation,
                ResourceGroupName = args.ResourceGroup.ResourceGroupName,
                ServerName = args.ServerArgs.ServerName,
            }, new()
            {
                DependsOn =
                {
                    args.ServerArgs.FlexibleServer,
                },
            });

        ConnectionString = args.ServerArgs.GetConnectionString(this);

        RegisterOutputs();
    }

    public string DatabaseName { get; }

    public Output<string>? ConnectionString { get; }
}