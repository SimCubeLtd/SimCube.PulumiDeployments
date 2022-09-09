namespace SimCube.PulumiDeployments.Resources.Kubernetes;

/// <inheritdoc />
public sealed class ServiceResource : ComponentResource
{
    private ServiceResource(NamespaceResource @namespace, string name, DeploymentConfiguration deploymentConfiguration, string defaultSelectorKeyValue, ComponentResourceOptions? options = null)
        : base(nameof(ServiceResource), name, options)
    {
        Guard.Against.Null(deploymentConfiguration, nameof(deploymentConfiguration));

        var customResourceOptions = new CustomResourceOptions {DependsOn = new List<Resource> {@namespace,},};

        Service = new(
            name,
            new()
            {
                Metadata = new ObjectMetaArgs
                {
                    Namespace = @namespace.NamespaceName,
                    Name = name,
                },
                Spec = CreateServiceSpec(deploymentConfiguration, defaultSelectorKeyValue),
            }, customResourceOptions);
    }

    public Service Service { get; }

    public static ServiceResource Create(NamespaceResource @namespace, string name, DeploymentConfiguration deploymentConfiguration, string defaultSelectorKeyValue, ComponentResourceOptions? componentResourceOptions = null)
    {
        Guard.Against.Null(deploymentConfiguration, nameof(deploymentConfiguration));

        var service = new ServiceResource(@namespace, name, deploymentConfiguration, defaultSelectorKeyValue, componentResourceOptions);

        componentResourceOptions?.DependsOn.Add(service.Service);

        return service;
    }

    private static ServiceSpecArgs CreateServiceSpec(DeploymentConfiguration deploymentConfiguration, string defaultSelectorKeyValue)
        => new()
        {
            Ports = new List<ServicePortArgs>
            {
                new()
                {
                    Name = deploymentConfiguration.Ports.Name,
                    Protocol = deploymentConfiguration.Ports.Protocol,
                    Port = deploymentConfiguration.Ports.Number,
                },
            },
            Type = deploymentConfiguration.ServiceType,
            Selector =
            {
                { KubernetesLiterals.DefaultSelectorKey, defaultSelectorKeyValue },
            },
        };
}