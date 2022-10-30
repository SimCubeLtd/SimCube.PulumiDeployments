namespace SimCube.PulumiDeployments.Arguments.Azure.PostgreSql.Flexible;

[ExcludeFromCodeCoverage]
public record PostgresFlexibleDatabaseResourceArgs(
    PostgresFlexibleServerResource ServerArgs,
    string? DatabaseName = null,
    string Collation = "en_US.utf8",
    string CharSet = "UTF8") : BaseAzureResourceArgs;