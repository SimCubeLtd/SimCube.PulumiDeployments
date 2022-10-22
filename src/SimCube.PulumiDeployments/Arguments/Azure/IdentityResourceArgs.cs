namespace SimCube.PulumiDeployments.Arguments.Azure;

public record IdentityResourceArgs(DateTime? ServicePrincipalPasswordExpiry = null) : BaseAzureResourceArgs;