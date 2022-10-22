using Pulumi.AzureNative.DBforPostgreSQL;

namespace SimCube.PulumiDeployments.Arguments.Azure;

[ExcludeFromCodeCoverage]
public record PostgresServerResourceArgs : BaseAzureResourceArgs
{
    public VirtualNetworkResource? VNet { get; init; }
    public SkuTier SkuTier { get; init; }

    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string ServerFamily { get; init; } = "Gen5";
    public string ServerFamilyName { get; init; } = "B_Gen5_1";
    public int ServerCapacity { get; init; } = 1;
    public string ServerVersion { get; init; } = "11.0";
    public int ServerBackupRetentionDays { get; init; } = 7;
    public bool ServerGeoRedundantBackup { get; init; }
    public int ServerStorageProfile { get; init; } = 5120;
}