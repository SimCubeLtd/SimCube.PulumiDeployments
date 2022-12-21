using Pulumi.AzureNative.Web;

namespace SimCube.PulumiDeployments.Resources.Azure.AppService;

[ExcludeFromCodeCoverage]
public sealed class ApiWebAppResource : BaseAzureResource<ApiWebAppResource, ApiWebAppResourceArgs>
{
    public ApiWebAppResource(
        string name,
        ApiWebAppResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(
            name,
            args,
            options)
    {
        var defaultApiWebAppName = $"{args.ApplicationName}-{ResourceNames.ApiWebApp}-{args.Location}-{args.Environment}";

        var webApp = new WebApp(
            args.ApiName ?? defaultApiWebAppName,
            new()
            {
                ResourceGroupName = args.ResourceGroup.ResourceGroupName,
                ServerFarmId = args.AppServicePlan.AppServicePlanId,
                Name = args.ApiName ?? defaultApiWebAppName,
                SiteConfig = args.SiteConfig,
                Tags = GetResourceTags,
            });

        WebAppName = webApp.Name;
        WebAppHostname = webApp.DefaultHostName;

        RegisterOutputs();
    }

    public Output<string> WebAppHostname { get; set; }

    public Output<string> WebAppName { get; set; }
}