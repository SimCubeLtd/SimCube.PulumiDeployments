using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;

namespace SimCube.PulumiDeployments.Helpers.Azure;

public static class StorageHelpers
{
    public static Output<string> GetSignedBlobReadUrl(
        Blob blob,
        BlobContainer container,
        StorageAccount account,
        ResourceGroup resourceGroup,
        DateTime? sasStartTime = null,
        DateTime? sasEndTime = null) =>
        Output.Tuple(blob.Name, container.Name, account.Name, resourceGroup.Name)
            .Apply(
                t =>
                {
                    var (blobName, containerName, accountName, resourceGroupName) = t;

                    var blobSAS = ListStorageAccountServiceSAS.InvokeAsync(
                        new()
                        {
                            AccountName = accountName,
                            Protocols = HttpProtocol.Https,
                            SharedAccessStartTime = sasStartTime?.ToString("yyyy-MM-dd") ?? DefaultSasStartTime,
                            SharedAccessExpiryTime = sasEndTime?.ToString("yyyy-MM-dd") ?? DefaultSasEndTime,
                            Resource = SignedResource.C,
                            ResourceGroupName = resourceGroupName,
                            Permissions = Permissions.R,
                            CanonicalizedResource = "/blob/" + accountName + "/" + containerName,
                            ContentType = "application/json",
                            CacheControl = "max-age=5",
                            ContentDisposition = "inline",
                            ContentEncoding = "deflate",
                        });

                    return Output.Format(
                        $"https://{accountName}.blob.core.windows.net/{containerName}/{blobName}?{blobSAS.GetAwaiter().GetResult().ServiceSasToken}");
                });

    private static string DefaultSasStartTime => DateTime.Now.Subtract(new TimeSpan(365, 0, 0, 0)).ToString("yyyy-MM-dd");
    private static string DefaultSasEndTime => DateTime.Now.AddDays(3650).ToString("yyyy-MM-dd");
}