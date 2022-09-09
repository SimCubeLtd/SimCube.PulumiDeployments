namespace SimCube.PulumiDeployments.Configuration;

public abstract class BaseHelmConfig
{
    public bool Enabled { get; init; } = true;

    public string VolumeSize { get; init; } = default!;

    public string HelmRepository { get; init; } = default!;

    public string HelmChartName { get; init; } = default!;

    public string? HelmChartVersion { get; init; }

    public string ImageRegistry { get; init; } = default!;

    public string ImageTag { get; init; } = default!;

    public string ImageRepository { get; init; } = default!;
}
