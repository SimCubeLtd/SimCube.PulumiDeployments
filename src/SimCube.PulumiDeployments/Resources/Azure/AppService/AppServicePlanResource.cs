using Pulumi.AzureNative.Web;

namespace SimCube.PulumiDeployments.Resources.Azure.AppService;

[ExcludeFromCodeCoverage]
public sealed class AppServicePlanResource : BaseAzureResource<AppServicePlanResource, AppServicePlanResourceArgs>
{
    public AppServicePlanResource(
        string name,
        AppServicePlanResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(name, args, options)
    {
        var defaultAppServicePlanName = $"{ResourceNames.AppServicePlan}-{args.Location}-{args.Environment}";

        var appServicePlan = new AppServicePlan(
            args.ServicePlanName ?? defaultAppServicePlanName,
            new()
            {
                Kind = args.Kind,
                Location = args.Location,
                Name = args.ServicePlanName ?? defaultAppServicePlanName,
                ResourceGroupName = args.ResourceGroup.ResourceGroupName,
                Reserved = args.IsReserved,
                Sku = args.Sku,
                Tags = GetResourceTags,
            });

        AppServicePlanId = appServicePlan.Id;
        AppServicePlanName = appServicePlan.Name;

        RegisterOutputs();
    }

    public Output<string> AppServicePlanId { get; }
    public Output<string> AppServicePlanName { get; }
}