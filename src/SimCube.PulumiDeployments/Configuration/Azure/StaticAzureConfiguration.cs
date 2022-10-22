namespace SimCube.PulumiDeployments.Configuration.Azure;

[ExcludeFromCodeCoverage]
public static class Azure
{
    public static IEnumerable<string> Locations =>
        JsonSerializer
            .Deserialize<AzureLocation[]>(
                Assets.Resources.AzureLocations,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                })!
            .Select(x => x.Name);

    /// <summary>
    /// Ideally these should be returned by the Azure CLI, so they can appear in AzureLocations.json.
    /// See https://github.com/Azure/azure-cli/issues/21579.
    /// </summary>
    public static IEnumerable<string> LocationsSupportingAvailabilityZones =>
        new[]
        {
            "brazilsouth",
            "francecentral",
            "southafrica",
            "northaustraliaeast",
            "canadacentral",
            "germanywestcentral",
            "centralindia",
            "centralus",
            "northeurope",
            "japaneast",
            "eastus",
            "norwayeast",
            "koreacentral",
            "eastus2",
            "uksouth",
            "southeastasia",
            "southcentralus",
            "westeurope",
            "eastasia",
            "usgovvirginia",
            "swedencentral",
            "chinanorth3",
            "westus2",
            "westus3",
        };

    private record struct AzureLocation(string Name);
}