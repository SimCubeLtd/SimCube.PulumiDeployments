using SimCube.PulumiDeployments.Arguments.Azure;

namespace SimCube.PulumiDeployments.Resources.Azure;

public class ResourceGroupResource : BaseAzureResource<ResourceGroupResource, ResourceGroupResourceArgs>
{
    public ResourceGroupResource(
        string name,
        ResourceGroupResourceArgs args,
        ComponentResourceOptions? options = null,
        CustomResourceOptions? resourceOptions = null)
        : base(name, args, options)
    {
        var resourceGroup = new Pulumi.AzureNative.Resources.ResourceGroup(
            $"{args.Configuration.ApplicationName}-{args.Location}-{args.Configuration.Environment}",
            new()
            {
                Location = args.Location,
                ResourceGroupName = $"{args.Configuration.ApplicationName}-{args.Location}-{args.Configuration.Environment}",
                Tags = args.Configuration.GetTags(args.Location),
            },
            resourceOptions);

        ResourceGroupId = resourceGroup.Id;

        RegisterOutputs();
    }

    public Output<string> ResourceGroupId { get; }
}