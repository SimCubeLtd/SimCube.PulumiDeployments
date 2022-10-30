using Pulumi.AzureNative.OperationalInsights;
using Pulumi.AzureNative.OperationalInsights.Inputs;

namespace SimCube.PulumiDeployments.Resources.Azure;

public sealed class MonitorResource : BaseAzureResource<MonitorResource, MonitorResourceArgs>
{
    public MonitorResource(
        string name,
        MonitorResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(name, args, options)
    {
        var workspaceName = $"{args.ApplicationName}-{ResourceNames.Workspace}-{args.Location}-{args.Environment}";

        var workspace = new Workspace(
            workspaceName,
            new()
            {
                WorkspaceName = workspaceName,
                Location = args.Location,
                ResourceGroupName = args.ResourceGroup.Name,
                RetentionInDays = args.RetentionDays,
                Sku = new WorkspaceSkuArgs
                {
                    Name = args.Sku,
                },
                Tags = GetResourceTags,
            });

        WorkspaceId = workspace.Id;
        WorkspaceName = workspace.Name;

        RegisterOutputs();
    }

    public Output<string> WorkspaceId { get; set; }

    public Output<string> WorkspaceName { get; set; }
}