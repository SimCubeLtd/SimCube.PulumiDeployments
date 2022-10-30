namespace SimCube.PulumiDeployments.Arguments.Azure.PostgreSql.Instance;

[ExcludeFromCodeCoverage]
public record PostgresInstanceDatabaseResourceArgs(PostgresInstanceServerResource ServerArgs, string? DatabaseName = null) : BaseAzureResourceArgs;