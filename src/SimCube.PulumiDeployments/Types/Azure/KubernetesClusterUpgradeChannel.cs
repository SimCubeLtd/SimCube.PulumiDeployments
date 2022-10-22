namespace SimCube.PulumiDeployments.Types.Azure;

public enum KubernetesClusterUpgradeChannel
{
    Rapid = 0,
    Stable = 1,
    Patch = 2,
    NodeImage = 3,
    None = 4,
}
