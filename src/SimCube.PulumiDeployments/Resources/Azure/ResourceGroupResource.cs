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
            $"{args.ApplicationName}-{args.Location}-{args.Environment}",
            new()
            {
                Location = args.Location,
                ResourceGroupName = $"{args.ApplicationName}-{args.Location}-{args.Environment}",
                Tags = args.GetTags(args.Location),
            },
            resourceOptions);

        ResourceGroupId = resourceGroup.Id;
        ResourceGroupName = resourceGroup.Name;

        RegisterOutputs();
    }

    public Output<string> ResourceGroupId { get; }
    public Output<string> ResourceGroupName { get; }
}