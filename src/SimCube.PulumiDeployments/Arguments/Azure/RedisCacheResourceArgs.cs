namespace SimCube.PulumiDeployments.Arguments.Azure;

[ExcludeFromCodeCoverage]
public record RedisCacheResourceArgs : BaseAzureResourceArgs
{
    public VirtualNetworkResource VNet { get; init; } = default!;
    public string MaxMemoryPolicy { get; init; } = "allkeys-lru";
    public int ReplicasPerMaster { get; set; } = 1;
    public int ShardCount { get; set; } = 1;
    public int SkuCapacity { get; set; } = 1;
    public string SkuFamily { get; set; } = "C";
    public string SkuName { get; set; } = "Enterprise_E10";
    public bool PublicNetworkAccess { get; init; }
    public bool EnableNonSslPort { get; init; }
}