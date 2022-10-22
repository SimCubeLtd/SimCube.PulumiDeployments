using Pulumi.AzureNative.DBforPostgreSQL;
using SimCube.PulumiDeployments.Arguments.Azure;

namespace SimCube.PulumiDeployments.Resources.Azure;

public sealed class PostgresInstanceDatabaseResource : BaseAzureResource<PostgresInstanceDatabaseResource, PostgresDatabaseResourceArgs>
{
    public PostgresInstanceDatabaseResource(
        string name,
        PostgresDatabaseResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(name, args, options)
    {
        var postgresDatabaseName = $"{ResourceNames.PostgresDatabase}-{args.Location}-{args.Environment}";

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
    public Database Database { get; }

    public Output<string> ConnectionString { get; }

    public static Output<string> GetConnectionString(PostgresDatabaseResourceArgs args)
    {
        ArgumentNullException.ThrowIfNull(args, nameof(args));

        return Output.Format(
            $"Host={args.ServerArgs.Server.FullyQualifiedDomainName};Database={args.DatabaseName};Username={args.Username}@{args.ServerArgs.Server.Name};Password={args.Password};Trust Server Certificate=True;SSL Mode=Require;");
    }
}