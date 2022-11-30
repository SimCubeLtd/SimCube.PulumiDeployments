using Pulumi.Kubernetes.Helm.V3;

namespace SimCube.PulumiDeployments.Resources.Helm;

public abstract class LocalHelmChartResource : BaseHelmChartResource
{
    protected LocalHelmChartResource(NamespaceResource @namespace, string type, string name, ComponentResourceOptions? options = null)
        : base(@namespace, type, name, options)
    {

    }

    protected Release CreateRelease(
        NamespaceResource @namespace,
        string chartName,
        string pathToChart,
        string? helmChartVersion = null,
        List<FileAsset>? helmValuesFiles = null,
        bool waitForJobs = true,
        bool skipAwait = false,
        int timeout = 1200)
          =>  new(
              chartName,
              new()
              {
                  Namespace = @namespace.NamespaceName,
                  Chart = pathToChart,
                  Version = helmChartVersion ?? string.Empty,
                  WaitForJobs = waitForJobs,
                  SkipAwait = skipAwait,
                  Timeout = timeout,
                  ValueYamlFiles = new List<AssetOrArchive>(helmValuesFiles ?? new List<FileAsset>()),
              },
              CustomResourceOptions);
}