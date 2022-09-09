namespace SimCube.PulumiDeployments.Configuration;

public sealed class PortConfiguration
{
    public string Name { get; init; } = default!;
    public string Protocol { get; init; } = default!;
    public int Number { get; init; }
}
