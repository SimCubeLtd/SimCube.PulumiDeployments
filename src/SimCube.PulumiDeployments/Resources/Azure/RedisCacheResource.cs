using Pulumi.AzureNative.Cache;
using Pulumi.AzureNative.Cache.Inputs;

namespace SimCube.PulumiDeployments.Resources.Azure;

public sealed class RedisCacheResource : BaseAzureResource<RedisCacheResource, RedisCacheResourceArgs>
{
    public RedisCacheResource(
        string name,
        RedisCacheResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(name, args, options)
    {
        var redisCacheName = $"{ResourceNames.RedisCache}-{args.Location}-{args.Environment}";

        var redisCache = new Redis(
            redisCacheName,
            new()
            {
                Name = redisCacheName,
                EnableNonSslPort = args.EnableNonSslPort,
                Location = args.Location,
                MinimumTlsVersion = TlsVersion.TlsVersion_1_2,
                RedisConfiguration =
                    new RedisCommonPropertiesRedisConfigurationArgs
                    {
                        MaxmemoryPolicy = args.MaxMemoryPolicy,
                    },
                ReplicasPerMaster = args.ReplicasPerMaster,
                ResourceGroupName = args.ResourceGroup.ResourceGroupName,
                ShardCount = args.ShardCount,
                Sku = new SkuArgs
                {
                    Capacity = args.SkuCapacity,
                    Family = args.SkuFamily,
                    Name = args.SkuName,
                },
                SubnetId = args.VNet.SubnetId,
                PublicNetworkAccess = args.PublicNetworkAccess ? PublicNetworkAccess.Enabled : PublicNetworkAccess.Disabled,
                Tags = GetResourceTags,
            });

        RedisCacheId = redisCache.Id;

        RegisterOutputs();
    }

    public Output<string> RedisCacheId { get; }
}