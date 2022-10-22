namespace SimCube.PulumiDeployments.Arguments.Azure;

[ExcludeFromCodeCoverage]
public record VirtualNetworkResourceArgs(bool IncludePublicIp, string? AddressPrefix = "") : BaseAzureResourceArgs;