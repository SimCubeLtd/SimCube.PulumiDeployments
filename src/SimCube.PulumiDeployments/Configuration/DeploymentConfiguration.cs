namespace SimCube.PulumiDeployments.Configuration;

public sealed class DeploymentConfiguration
{
    public IngressConfiguration[] Ingress { get; init; } = Array.Empty<IngressConfiguration>();
    public PortConfiguration Ports { get; init; } = default!;
    public string Image { get; set; } = default!;
    public string ServiceType { get; init; } = "ClusterIP";
    public string ServiceName { get; init; } = default!;
    public int Replicas { get; init; } = 1;
}
