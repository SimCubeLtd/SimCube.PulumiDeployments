using Pulumi.AzureNative.DBforPostgreSQL;
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
        Initialise(args);

        RegisterOutputs();
    }

    protected override Server CreateServer(PostgresServerResourceArgs args)
    {
        ArgumentNullException.ThrowIfNull(AdminPassword, nameof(AdminPassword));

        return new(
            ServerName,
            new()
            {
                ServerName = ServerName,
                Location = args.Location,
                Properties = new ServerPropertiesForDefaultCreateArgs
                {
                    AdministratorLogin = Username,
                    AdministratorLoginPassword = AdminPassword.Result,
                    CreateMode = CreateMode.Default.ToString(),
                    MinimalTlsVersion = MinimalTlsVersionEnum.TLS1_2,
                    SslEnforcement = SslEnforcementEnum.Enabled,
                    StorageProfile = new StorageProfileArgs
                    {
                        BackupRetentionDays = args.ServerBackupRetentionDays,
                        GeoRedundantBackup = args.ServerGeoRedundantBackup ? GeoRedundantBackup.Enabled : GeoRedundantBackup.Disabled,
                        StorageMB = args.ServerStorageProfile,
                    },
                    PublicNetworkAccess = PublicNetworkAccessEnum.Enabled,
                    Version = args.ServerVersion,
                },
                ResourceGroupName = args.ResourceGroup.Name,
                Sku = new SkuArgs
                {
                    Capacity = args.ServerCapacity, Family = args.ServerFamily, Name = args.ServerFamilyName, Tier = args.SkuTier,
                },
                Tags = GetResourceTags,
            });
    }


    public override Output<string> GetConnectionString(PostgresDatabaseResource databaseResource) =>
        Output.Format(
            $"Host={Server?.FullyQualifiedDomainName};Database={databaseResource.Database.Name};Username={Username}@{Server?.Name};Password={AdminPassword?.Result};Trust Server Certificate=True;SSL Mode=Require;");
}