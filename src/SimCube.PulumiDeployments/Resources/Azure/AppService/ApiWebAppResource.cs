using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;

namespace SimCube.PulumiDeployments.Resources.Azure.AppService;

[ExcludeFromCodeCoverage]
public sealed class ApiWebAppResource : BaseAzureResource<ApiWebAppResource, ApiWebAppResourceArgs>
{
    private readonly Input<string> _websitePackage = "WEBSITE_RUN_FROM_PACKAGE";

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
        var defaultEndpoint = $"https://{args.ApplicationName}-{ResourceNames.UiWebApp}-{args.Location}-{args.Environment}.azurewebsites.net";

        PopulateAppSettingsWithBlobUrl(args);
        PopulateCorsConfigWithExtraHosts(args, defaultEndpoint);

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

    private static void PopulateCorsConfigWithExtraHosts(ApiWebAppResourceArgs args, string defaultEndpoint)
    {
        var corsConfig = new CorsSettingsArgs
        {
            SupportCredentials = args.CorsSupportsCredentials,
            AllowedOrigins = new List<string>()
            {
                args.EndpointAddress ?? defaultEndpoint,
            },
        };

        if (args.ExtraOrigins?.Any() == true)
        {
            corsConfig.AllowedOrigins.AddRange(args.ExtraOrigins);
        }

        args.SiteConfig.Cors = corsConfig;
    }

    private void PopulateAppSettingsWithBlobUrl(ApiWebAppResourceArgs args)
    {
        var appSettings = args.SiteConfig.AppSettings.Cast<NameValuePairArgs>().ToList();

        if (appSettings.Any(x => x.Name == _websitePackage) || args.DeploymentBlob.BlobReadUrl is null)
        {
            return;
        }

        appSettings.Add(new()
        {
            Name = _websitePackage,
            Value = args.DeploymentBlob.BlobReadUrl,
        });

        args.SiteConfig.AppSettings = appSettings;
    }

    public Output<string> WebAppHostname { get; set; }

    public Output<string> WebAppName { get; set; }
}