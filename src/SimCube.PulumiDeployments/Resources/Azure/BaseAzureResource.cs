using SimCube.PulumiDeployments.Arguments.Azure;

namespace SimCube.PulumiDeployments.Resources.Azure;

public abstract class BaseAzureResource<TType, TArgs> : ComponentResource
    where TArgs : BaseAzureResourceArgs
{
    private readonly TArgs _args;

    protected BaseAzureResource(
        string name,
        TArgs args,
        ComponentResourceOptions? options = null)
        : base(GetType(args), GetName(name, args), options) =>
        _args = args;

    protected Dictionary<string, string> GetResourceTags => _args.GetTags(_args.Location);

    private static string GetType(TArgs args) => $"{args.ApplicationName}:{typeof(TType).Name}";

    private static string GetName(
        string name,
        TArgs args) =>
        $"{name}-{GetTypeName()}-{(string.IsNullOrEmpty(args.Location) ? string.Empty : $"{args.Location}-")}{args.Environment}";

    private static string GetTypeName() => typeof(TType).Name.ToLowerInvariant().Replace("Resource", string.Empty, StringComparison.Ordinal);
}