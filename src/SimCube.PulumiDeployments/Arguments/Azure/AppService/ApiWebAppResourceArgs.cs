using Pulumi.AzureNative.Web.Inputs;
using SimCube.PulumiDeployments.Resources.Azure.AppService;
using SimCube.PulumiDeployments.Resources.Azure.Storage;

namespace SimCube.PulumiDeployments.Arguments.Azure.AppService;

public record ApiWebAppResourceArgs : BaseAzureResourceArgs
{
    public BlobStorageResource BlobStorage { get; set; } = default!;
    public BlobContainerResource Container { get; set; } = default!;
    public AppServicePlanResource AppServicePlan { get; set; } = default!;
    public SiteConfigArgs SiteConfig { get; set; } = DefaultSiteConfig;
    public BlobResource DeploymentBlob { get; set; } = default!;
    public string? ApiName { get; set; } = default!;
    public string? EndpointAddress { get; set; } = default!;
    public List<string>? ExtraOrigins { get; init; } = default!;
    public bool CorsSupportsCredentials { get; init; } = true;

    private static SiteConfigArgs DefaultSiteConfig => new()
    {
        LinuxFxVersion = "DOTNETCORE|7.0",
        AlwaysOn = true,
        Http20Enabled = true,
        WebSocketsEnabled = true,
        AppSettings = new List<NameValuePairArgs>(),
    };
}