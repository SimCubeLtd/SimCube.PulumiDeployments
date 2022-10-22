using Pulumi.AzureNative.Cache;
using Pulumi.AzureNative.Cache.Inputs;
using SimCube.PulumiDeployments.Arguments.Azure;

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
                ResourceGroupName = args.ResourceGroup.Name,
                ShardCount = args.ShardCount,
                Sku = new SkuArgs
                {
                    Capacity = args.SkuCapacity,
                    Family = args.SkuFamily,
                    Name = args.SkuName,
                },
                SubnetId = args.VNet.Subnet.Id,
                PublicNetworkAccess = args.PublicNetworkAccess ? PublicNetworkAccess.Enabled : PublicNetworkAccess.Disabled,
                Tags = GetResourceTags,
            });

        RedisCacheId = redisCache.Id;

        RegisterOutputs();
    }

    public Output<string> RedisCacheId { get; }
}