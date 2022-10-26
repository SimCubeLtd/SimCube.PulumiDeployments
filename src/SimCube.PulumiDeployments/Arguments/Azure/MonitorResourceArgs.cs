using Pulumi.AzureNative.OperationalInsights;

namespace SimCube.PulumiDeployments.Arguments.Azure;

[ExcludeFromCodeCoverage]
public record MonitorResourceArgs : BaseAzureResourceArgs
{
    public int RetentionDays { get; init; } = 30;
    public WorkspaceSkuNameEnum Sku { get; init; } = WorkspaceSkuNameEnum.Standard;
}