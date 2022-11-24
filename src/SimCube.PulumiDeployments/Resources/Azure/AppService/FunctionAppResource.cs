using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;

namespace SimCube.PulumiDeployments.Resources.Azure.AppService;

[ExcludeFromCodeCoverage]
public sealed class FunctionAppResource : BaseAzureResource<FunctionAppResource, FunctionAppResourceArgs>
{
    public FunctionAppResource(
        string name,
        FunctionAppResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(
            name,
            args,
            options)
    {
        var defaultFunctionAppName = $"{args.ApplicationName}-{ResourceNames.ApiWebApp}-{args.Location}-{args.Environment}";
       
        var webApp = new WebApp(
            args.FunctionName ?? defaultFunctionAppName,
            new()
            {
                Kind = "FunctionApp",
                ResourceGroupName = args.ResourceGroup.ResourceGroupName,
                ServerFarmId = args.AppServicePlan.AppServicePlanId,
                Name = args.FunctionName ?? defaultFunctionAppName,
                SiteConfig = args.SiteConfig ?? new SiteConfigArgs(),
                Tags = GetResourceTags,
            });

        FunctionAppName = webApp.Name;
        FunctionAppHostname = webApp.DefaultHostName;

        RegisterOutputs();
    }

    public Output<string> FunctionAppHostname { get; set; }

    public Output<string> FunctionAppName { get; set; }
}