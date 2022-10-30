namespace SimCube.PulumiDeployments.Arguments.Azure.PostgreSql.Flexible;

[ExcludeFromCodeCoverage]
public record PostgresFlexibleDatabaseResourceArgs(PostgresFlexibleServerResource ServerArgs, string? DatabaseName = null) : BaseAzureResourceArgs;