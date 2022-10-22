namespace SimCube.PulumiDeployments.Arguments.Azure;

[ExcludeFromCodeCoverage]
public record PostgresDatabaseResourceArgs(PostgresServerResource ServerArgs, string DatabaseName, string Username, string Password) : BaseAzureResourceArgs;