using Pulumi.AzureNative.Web.Inputs;
using SimCube.PulumiDeployments.Resources.Azure.AppService;
using SimCube.PulumiDeployments.Resources.Azure.Storage;

namespace SimCube.PulumiDeployments.Arguments.Azure.AppService;

public record FunctionAppResourceArgs : BaseAzureResourceArgs
{
    public AppServicePlanResource AppServicePlan { get; set; } = default!;
    public SiteConfigArgs? SiteConfig { get; init; } = default!;
    public BlobResource DeploymentBlob { get; set; } = default!;
    public string? FunctionName { get; set; } = default!;
    public string? EndpointAddress { get; set; } = default!;
    public List<string>? ExtraOrigins { get; init; } = default!;
    public bool CorsSupportsCredentials { get; init; } = true;
}