namespace SimCube.PulumiDeployments.Arguments.Azure.PostgreSql.Instance;

[ExcludeFromCodeCoverage]
public record PostgresInstanceDatabaseResourceArgs(
    PostgresInstanceServerResource ServerArgs,
    string? DatabaseName = null,
    string Collation = "en_US.utf8",
    string CharSet = "UTF8") : BaseAzureResourceArgs;