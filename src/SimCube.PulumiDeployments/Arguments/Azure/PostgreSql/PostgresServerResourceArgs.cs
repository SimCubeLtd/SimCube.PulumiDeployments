using Pulumi.AzureNative.DBforPostgreSQL;

namespace SimCube.PulumiDeployments.Arguments.Azure.PostgreSql;

[ExcludeFromCodeCoverage]
public record PostgresServerResourceArgs : BaseAzureResourceArgs
{
    public VirtualNetworkResource? VNet { get; init; }
    public SkuTier SkuTier { get; init; }

    public string? Username { get; init; } = default!;
    public string ServerFamily { get; init; } = "Burstable";
    public string ServerFamilyName { get; init; } = "Standard_B1ms";
    public int ServerCapacity { get; init; } = 1;
    public string ServerVersion { get; init; } = "14.0";
    public int ServerBackupRetentionDays { get; init; } = 7;
    public bool ServerGeoRedundantBackup { get; init; }
    public int ServerStorageProfile { get; init; } = 32768;

    public bool ShouldCreateFirewallRules { get; init; } = true;
    public Dictionary<string, string> FirewallAllowedIpAddresses { get; init; } = new();
}