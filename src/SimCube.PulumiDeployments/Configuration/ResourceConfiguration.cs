namespace SimCube.PulumiDeployments.Configuration;

[ExcludeFromCodeCoverage]
public sealed class ResourceConfiguration
{
    public string CpuRequest { get; init; } = default!;

    public string MemoryRequest { get; init; } = default!;

    public string CpuLimit { get; init; } = default!;

    public string MemoryLimit { get; init; } = default!;
}
