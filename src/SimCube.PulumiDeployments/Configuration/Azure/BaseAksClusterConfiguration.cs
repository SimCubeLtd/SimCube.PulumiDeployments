namespace SimCube.PulumiDeployments.Configuration.Azure;

public abstract class BaseAksClusterConfiguration
{
    public string KubernetesVersion { get; set; } = default!;
    public string ManagementGroupId { get; set; } = default!;
    public string NodeVmSize { get; set; } = default!;
    public int NumberWorkerNodes { get; set; } = default!;
    public string DnsPrefix { get; set; } = default!;
    public string? SshPublicKey { get; set; }
    public string Location { get; set; } = default!;
}