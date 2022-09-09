namespace SimCube.PulumiDeployments.Resources.Kubernetes;

[SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "I Want them.")]
[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "I Want them.")]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "I Want them.")]
public abstract class BaseKubernetesDeploymentResource : ComponentResource
{
    protected readonly ComponentResourceOptions? ComponentResourceOptions;
    protected readonly CustomResourceOptions CustomResourceOptions;
    protected readonly NamespaceResource NamespaceResource;
    protected readonly string Name;
    protected readonly string DefaultSelectorKeyValue;

    protected BaseKubernetesDeploymentResource(
        NamespaceResource @namespace,
        string type,
        string name,
        string defaultSelectorKeyValue,
        ComponentResourceOptions? options = null)
        : base(type, name, options)
    {
        NamespaceResource = @namespace;
        CustomResourceOptions = new() {DependsOn = new List<Resource> {@namespace,},};
        ComponentResourceOptions = options;
        Name = name;
        DefaultSelectorKeyValue = defaultSelectorKeyValue;
    }

    protected abstract string InstanceName { get; }

    protected DeploymentResource CreateDeploymentResource(DeploymentConfiguration deploymentConfiguration, List<EnvVarArgs>? envVariables = null)
        => DeploymentResource.Create(NamespaceResource, $"{InstanceName}-deployment", deploymentConfiguration, DefaultSelectorKeyValue, envVariables, ComponentResourceOptions);

    protected ServiceResource CreateServiceResource(DeploymentConfiguration deploymentConfiguration)
        => ServiceResource.Create(NamespaceResource, $"{InstanceName}-service", deploymentConfiguration, DefaultSelectorKeyValue, ComponentResourceOptions);

    protected IngressResource CreateIngressResource(IngressConfiguration[] configuration)
        => IngressResource.Create(NamespaceResource, $"{InstanceName}-ingress", configuration, $"{InstanceName}-service", ComponentResourceOptions);
}