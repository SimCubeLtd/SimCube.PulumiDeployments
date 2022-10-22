using Pulumi.AzureAD;
using SimCube.PulumiDeployments.Arguments.Azure;

namespace SimCube.PulumiDeployments.Resources.Azure;

public sealed class IdentityResource : BaseAzureResource<IdentityResource, IdentityResourceArgs>
{
    public IdentityResource(
        string name,
        IdentityResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(name, args, options)
    {
        var azureActiveDirectoryTags = args.Configuration.GetAzureActiveDirectoryTags();
        var azureActiveDirectoryDescription = args.Configuration.GetAzureActiveDirectoryDescription();

        var adAppName = args.ApplicationName;
        var adServicePrincipalName = $"{args.ApplicationName}-{ResourceNames.ServicePrincipal}";
        var adServicePrincipalPasswordName = $"{adServicePrincipalName}-password";

        var application = new Application(
            adAppName,
            new()
            {
                DisplayName = adAppName,
                SupportUrl = args.SupportAddress ?? string.Empty,
                Tags = azureActiveDirectoryTags,
            });

        var servicePrincipal = new ServicePrincipal(
            adServicePrincipalName,
            new()
            {
                ApplicationId = application.ApplicationId,
                Description = azureActiveDirectoryDescription,
                Notes = azureActiveDirectoryDescription,
                Tags = azureActiveDirectoryTags,
            });

        var servicePrincipalPassword = new ServicePrincipalPassword(
            adServicePrincipalPasswordName,
            new()
            {
                // cannot be changed once created, so long expiry is fine.
                EndDate = args.ServicePrincipalPasswordExpiry?.ToRfc3339String() ?? new DateTime(2999, 1, 1).ToRfc3339String(),
                ServicePrincipalId = servicePrincipal.Id,
            });

        ClientId = application.ApplicationId;
        ClientSecret = servicePrincipalPassword.Value;

        RegisterOutputs();
    }

    public Output<string> ClientId { get; }

    public Output<string> ClientSecret { get; }
}