namespace SimCube.PulumiDeployments.Resources.Kubernetes;

/// <inheritdoc />
public sealed class NamespaceResource : ComponentResource
{
    private NamespaceResource(string namespaceName, ComponentResourceOptions? options = null)
        : base(nameof(NamespaceResource), namespaceName, options)
    {
        Namespace = new(
            namespaceName,
            new()
            {
                Metadata = new ObjectMetaArgs
                {
                    Name = namespaceName,
                },
            });

        NamespaceName = namespaceName;
    }

    /// <summary>
    /// Creates a new <see cref="NamespaceResource"/>.
    /// </summary>
    public Namespace Namespace { get; }

    /// <summary>
    /// The name of the namespace.
    /// </summary>
    public string NamespaceName { get; }

    /// <summary>
    /// Creates a new <see cref="NamespaceResource"/>.
    /// </summary>
    /// <param name="namespaceName">the namespace name.</param>
    /// <param name="componentResourceOptions">the resource options.</param>
    /// <returns></returns>
    public static NamespaceResource Create(string namespaceName, ComponentResourceOptions componentResourceOptions)
    {
        Guard.Against.Null(componentResourceOptions, nameof(componentResourceOptions));

        var @namespace = new NamespaceResource(namespaceName, componentResourceOptions);

        componentResourceOptions.DependsOn.Add(@namespace.Namespace);

        return @namespace;
    }
}
