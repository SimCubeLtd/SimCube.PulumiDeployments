using SimCube.PulumiDeployments.Resources.Azure.PostgreSql;

namespace SimCube.PulumiDeployments.Arguments.Azure.PostgreSql;

[ExcludeFromCodeCoverage]
public record PostgresDatabaseResourceArgs(BasePostgresServerInstance ServerArgs, string? DatabaseName = null) : BaseAzureResourceArgs;