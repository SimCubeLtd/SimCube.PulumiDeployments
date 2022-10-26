using Pulumi.AzureNative.Web.Inputs;

namespace SimCube.PulumiDeployments.Arguments.Azure.AppService;

public record AppServicePlanResourceArgs : BaseAzureResourceArgs
{
    public string? ServicePlanName { get; set; } = default!;
    public string Kind { get; init; } = "Linux";
    public bool IsReserved { get; init; }

    public SkuDescriptionArgs Sku { get; init; } = DefaultSku;

    private static readonly SkuDescriptionArgs DefaultSku = new()
    {
        Size = "B1",
        Tier = "Basic",
        Name = "B1",
    };
}