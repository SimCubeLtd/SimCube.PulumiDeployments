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

        var network = CreateNetwork(args, vnetName);

        var subnet = CreateSubnet(args, subnetName, network.Name);

        if (args.IncludePublicIp)
        {
            var ipAddress = new PublicIPAddress(
                publicIpName,
                new()
                {
                    Location = args.Location,
                    PublicIpAddressName = publicIpName,
                    ResourceGroupName = args.ResourceGroup.ResourceGroupName,
                    Tags = GetResourceTags,
                });

            PublicIpAddress = ipAddress.IpAddress;
        }

        VirtualNetworkId = network.Id;
        VirtualNetworkName = network.Name;
        SubnetId = subnet.Id;
        SubnetName = subnet.Name;

        RegisterOutputs();
    }

    private static Subnet CreateSubnet(VNetArgs args, string subnetName,Output<string> vnetName) =>
        new(
            subnetName,
            new()
            {
                SubnetName = subnetName,
                AddressPrefix = args.AddressPrefix ?? string.Empty,
                ResourceGroupName = args.ResourceGroup.ResourceGroupName,
                VirtualNetworkName = vnetName,
            });

    private VirtualNetwork CreateNetwork(VNetArgs args, string vnetName) =>
        new(
            vnetName,
            new()
            {
                VirtualNetworkName = vnetName,
                AddressSpace = new AddressSpaceArgs
                {
                    AddressPrefixes =
                    {
                        args.AddressPrefix ?? string.Empty,
                    },
                },
                Location = args.Location,
                ResourceGroupName = args.ResourceGroup.ResourceGroupName,
                Tags = GetResourceTags,
            });

    public Output<string> VirtualNetworkId { get; }

    public Output<string> VirtualNetworkName { get; }

    public Output<string> SubnetId { get; }
    public Output<string?> SubnetName { get; }

    public Output<string?> PublicIpAddress { get; } = default!;
}