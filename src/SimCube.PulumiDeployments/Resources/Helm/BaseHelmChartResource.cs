﻿namespace SimCube.PulumiDeployments.Resources.Helm;

[SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "I Want them.")]
[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "I Want them.")]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "I Want them.")]
public abstract class BaseHelmChartResource : ComponentResource
{
    protected readonly CustomResourceOptions CustomResourceOptions;

    private const string EnvSubstitute = "envsubst";
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

    protected static string RenderCommandName(string chartName) => $"render-values-{chartName}";

    protected Command RenderYamlValues(string commandName, string helmFile, InputMap<string> environment)
    {
        var createCommand = new Command(
            commandName,
            new()
            {
                Create = RenderCreateCommand(helmFile),
                Environment = environment,
            },
            CustomResourceOptions);

        CustomResourceOptions.DependsOn.Add(createCommand);

        return createCommand;
    }

    protected string GetHelmValuesFilePath() => Path.Combine(AppContext.BaseDirectory, HelmValuesFolder, HelmValuesFile);

    private static string RenderCreateCommand(string helmValuePath) => $"{EnvSubstitute} < {helmValuePath} > {helmValuePath}.new && mv {helmValuePath}.new {helmValuePath}";
}
