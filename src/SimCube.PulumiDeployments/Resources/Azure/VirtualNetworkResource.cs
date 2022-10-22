using Pulumi.AzureNative.Network;
using Pulumi.AzureNative.Network.Inputs;
using VNetArgs = SimCube.PulumiDeployments.Arguments.Azure.VirtualNetworkResourceArgs;

namespace SimCube.PulumiDeployments.Resources.Azure;

public sealed class VirtualNetworkResource : BaseAzureResource<VirtualNetworkResource, VNetArgs>
{
    public VirtualNetworkResource(
        string name,
        VNetArgs args,
        ComponentResourceOptions? options = null)
        : base(name, args, options)
    {
        var vnetName = $"{args.Configuration.ApplicationName}-{ResourceNames.VirtualNetwork}-{args.Location}-{args.Configuration.Environment}";
        var subnetName = $"{args.Configuration.ApplicationName}-{ResourceNames.Subnet}-{args.Location}-{args.Configuration.Environment}";
        var publicIpName = $"{args.Configuration.ApplicationName}-{ResourceNames.PublicIpAddress}-{args.Location}-{args.Configuration.Environment}";

        Network = new(
            vnetName,
            new()
            {
                VirtualNetworkName = vnetName,
                AddressSpace = new AddressSpaceArgs()
                {
                    AddressPrefixes =
                    {
                        args.Configuration.VirtualNetwork.AddressPrefix,
                    },
                },
                Location = args.Location,
                ResourceGroupName = args.ResourceGroup.Name,
                Tags = GetResourceTags,
            });

        Subnet = new(
            subnetName,
            new()
            {
                SubnetName = subnetName,
                AddressPrefix = args.Configuration.VirtualNetwork.AddressPrefix,
                ResourceGroupName = args.ResourceGroup.Name,
                VirtualNetworkName = vnetName,
            },
            new()
            {
                DependsOn = new()
                {
                    Network,
                },
            });

        if (args.Configuration.VirtualNetwork.IncludePublicIp)
        {
            PublicIp = new(
                publicIpName,
                new()
                {
                    Location = args.Location,
                    PublicIpAddressName = publicIpName,
                    ResourceGroupName = args.ResourceGroup.Name,
                    Tags = GetResourceTags,
                });
        }

        RegisterOutputs();
    }

    public VirtualNetwork Network { get; }

    public Subnet Subnet { get; }

    public PublicIPAddress? PublicIp { get; }
}