namespace SimCube.PulumiDeployments.Configuration;

[SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "I Want to.")]
[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "I Want to.")]
public abstract class IacConfiguration
{
    protected readonly Config Config = new();

    public string ApplicationName => Config.GetString(nameof(ApplicationName));

    public string NamespaceName => Config.GetString(nameof(NamespaceName));

    public string StorageClassName => Config.GetString(nameof(StorageClassName));

    public string IngressClassName => Config.GetString(nameof(IngressClassName));
}
