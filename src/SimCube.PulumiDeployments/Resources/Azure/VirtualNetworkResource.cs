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
        var vnetName = $"{args.ApplicationName}-{ResourceNames.VirtualNetwork}-{args.Location}-{args.Environment}";
        var subnetName = $"{args.ApplicationName}-{ResourceNames.Subnet}-{args.Location}-{args.Environment}";
        var publicIpName = $"{args.ApplicationName}-{ResourceNames.PublicIpAddress}-{args.Location}-{args.Environment}";

        Network = CreateNetwork(args, vnetName);

        Subnet = CreateSubnet(args, subnetName, vnetName);

        if (args.IncludePublicIp == true)
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

    private Subnet CreateSubnet(VNetArgs args, string subnetName, string vnetName) =>
        new(
            subnetName,
            new()
            {
                SubnetName = subnetName,
                AddressPrefix = args.AddressPrefix ?? string.Empty,
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

    private VirtualNetwork CreateNetwork(VNetArgs args, string vnetName) =>
        new(
            vnetName,
            new()
            {
                VirtualNetworkName = vnetName,
                AddressSpace = new AddressSpaceArgs() {
                    AddressPrefixes =
                    {
                        args.AddressPrefix ?? string.Empty,
                    },
                },
                Location = args.Location,
                ResourceGroupName = args.ResourceGroup.Name,
                Tags = GetResourceTags,
            });

    public VirtualNetwork Network { get; }

    public Subnet Subnet { get; }

    public PublicIPAddress? PublicIp { get; }
}