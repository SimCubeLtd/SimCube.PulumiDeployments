using Pulumi.AzureNative.OperationalInsights;
using Pulumi.AzureNative.OperationalInsights.Inputs;
using SimCube.PulumiDeployments.Arguments.Azure;

namespace SimCube.PulumiDeployments.Resources.Azure;

public sealed class MonitorResource : BaseAzureResource<MonitorResource, MonitorResourceArgs>
{
    public MonitorResource(
        string name,
        MonitorResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(name, args, options)
    {
        var workspaceName = $"{args.Configuration.ApplicationName}-{ResourceNames.Workspace}-{args.Location}-{args.Configuration.Environment}";

        var workspace = new Workspace(
            workspaceName,
            new()
            {
                WorkspaceName = workspaceName,
                Location = args.Location,
                ResourceGroupName = args.ResourceGroup.Name,
                RetentionInDays = 30,
                Sku = new WorkspaceSkuArgs()
                {
                    Name = WorkspaceSkuNameEnum.PerGB2018,
                },
                Tags = GetResourceTags,
            });

        WorkspaceId = workspace.Id;

        RegisterOutputs();
    }

    public Output<string> WorkspaceId { get; set; }
}