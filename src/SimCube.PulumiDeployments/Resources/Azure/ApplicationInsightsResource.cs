using Pulumi.AzureNative.Insights;

namespace SimCube.PulumiDeployments.Resources.Azure;

public sealed class ApplicationInsightsResource : BaseAzureResource<ApplicationInsightsResource, ApplicationInsightsResourceArgs>
{
    public ApplicationInsightsResource(
        string name,
        ApplicationInsightsResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(name, args, options)
    {
        var instanceName = $"{args.ApplicationName}-{ResourceNames.ApplicationInsights}-{args.Location}-{args.Environment}";

        var appInsights = new Component(
            args.Name ?? instanceName,
            new()
        {
            ApplicationType = args.ApplicationType,
            Kind = args.Kind,
            ResourceGroupName = args.ResourceGroup.ResourceGroupName,
        });

        ApplicationInsightsId = appInsights.Id;
        ApplicationInsightsName = appInsights.Name;
        ApplicationInsightsInstrumentationKey = appInsights.InstrumentationKey;
        
        RegisterOutputs();
    }

    public Output<string> ApplicationInsightsId { get; set; }
    public Output<string> ApplicationInsightsName { get; set; }
    public Output<string> ApplicationInsightsInstrumentationKey { get; set; }
}