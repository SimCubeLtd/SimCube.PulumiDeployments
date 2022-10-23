using SimCube.PulumiDeployments.Resources.Azure.PostgreSql;

namespace SimCube.PulumiDeployments.Arguments.Azure.PostgreSql;

[ExcludeFromCodeCoverage]
public record PostgresDatabaseResourceArgs(BasePostgresSqlServerResource ServerArgs, string? DatabaseName = null) : BaseAzureResourceArgs;