namespace SimCube.PulumiDeployments.Resources.Helm;

[SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "I Want them.")]
[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "I Want them.")]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "I Want them.")]
public abstract class BaseHelmChartResource : ComponentResource
{
    protected readonly CustomResourceOptions CustomResourceOptions;

    private const string EnvSubstituteCommand = "envsubst";
    private const string MoveCommand = "mv";
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

    protected string RenderYamlValues(Dictionary<string, string?> environmentalVariables)
    {
        var helmValuesFile = GetHelmValuesFilePath();

        Cli.Wrap(EnvSubstituteCommand)
            .WithArguments($"< {helmValuesFile} > {helmValuesFile}.new")
            .WithEnvironmentVariables(environmentalVariables)
            .WithValidation(CommandResultValidation.ZeroExitCode)
            .ExecuteAsync()
            .GetAwaiter()
            .GetResult();

        Cli.Wrap(MoveCommand)
            .WithArguments($"{helmValuesFile}.new {helmValuesFile}")
            .WithEnvironmentVariables(environmentalVariables)
            .WithValidation(CommandResultValidation.ZeroExitCode)
            .ExecuteAsync()
            .GetAwaiter()
            .GetResult();

        return helmValuesFile;
    }

    private string GetHelmValuesFilePath() => Path.Combine(AppContext.BaseDirectory, HelmValuesFolder, HelmValuesFile);
}