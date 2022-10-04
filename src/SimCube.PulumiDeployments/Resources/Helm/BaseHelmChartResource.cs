using Pulumi.Kubernetes.Helm.V3;
using Pulumi.Kubernetes.Types.Inputs.Helm.V3;

namespace SimCube.PulumiDeployments.Resources.Helm;

[SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "I Want them.")]
[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "I Want them.")]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "I Want them.")]
public abstract class BaseHelmChartResource : ComponentResource
{
    protected readonly CustomResourceOptions CustomResourceOptions;
    public const string HelmValuesFolder = "HelmValues";

    protected BaseHelmChartResource(
        NamespaceResource @namespace,
        string type,
        string name,
        ComponentResourceOptions? options = null)
        : base(type, name, options) =>
        CustomResourceOptions = new()
        {
            DependsOn = new List<Resource>
            {
                @namespace,
            },
        };

    protected abstract string HelmValuesFile { get; }

    protected List<FileAsset> RenderYamlValues(Dictionary<string, string?> environmentalVariables)
    {
        var helmValuesFile = GetHelmValuesFilePath();

        foreach (var environmentalVariable in environmentalVariables)
        {
            Environment.SetEnvironmentVariable(environmentalVariable.Key, environmentalVariable.Value);
        }

        var origin = File.ReadAllText(helmValuesFile);
        var output = Environment.ExpandEnvironmentVariables(origin);

        File.WriteAllText(helmValuesFile, output);

        return new()
        {
            new(helmValuesFile),
        };
    }

    protected Release CreateRelease(
        NamespaceResource @namespace,
        string chartName,
        string helmChartName,
        string helmRepository,
        string? helmChartVersion = null,
        List<FileAsset>? helmValuesFiles = null,
        bool waitForJobs = true,
        bool skipAwait = false,
        int timeout = 1200) =>
        new(
            chartName,
            new()
            {
                Namespace = @namespace.NamespaceName,
                Chart = helmChartName,
                Version = helmChartVersion ?? string.Empty,
                WaitForJobs = waitForJobs,
                SkipAwait = skipAwait,
                Timeout = timeout,
                RepositoryOpts = new RepositoryOptsArgs
                {
                    Repo = helmRepository,
                },
                ValueYamlFiles = new List<AssetOrArchive>(helmValuesFiles ?? new List<FileAsset>()),
            },
            CustomResourceOptions);

    private string GetHelmValuesFilePath() => Path.Combine(AppContext.BaseDirectory, HelmValuesFolder, HelmValuesFile);
}