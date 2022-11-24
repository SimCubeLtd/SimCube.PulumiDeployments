using Pulumi.AzureNative.Insights;

namespace SimCube.PulumiDeployments.Arguments.Azure;

[ExcludeFromCodeCoverage]
public record ApplicationInsightsResourceArgs : BaseAzureResourceArgs
{
    public ApplicationType ApplicationType { get; init; } = ApplicationType.Web;
    public string Kind { get; init; } = "web";
    public string? Name { get; set; }
}