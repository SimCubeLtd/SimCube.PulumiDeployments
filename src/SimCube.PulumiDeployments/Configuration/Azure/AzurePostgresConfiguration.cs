namespace SimCube.PulumiDeployments.Configuration.Azure;

[ExcludeFromCodeCoverage]
public abstract class AzurePostgresConfiguration
{
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string DatabaseName { get; init; } = default!;
    public string ServerFamily { get; init; } = "Gen5";
    public string ServerFamilyName { get; init; } = "B_Gen5_1";
    public int ServerCapacity { get; init; } = 1;
    public string ServerVersion { get; init; } = "11.0";
    public int ServerBackupRetentionDays { get; init; } = 7;
    public bool ServerGeoRedundantBackup { get; init; }
    public int ServerStorageProfile { get; init; } = 5120;
}