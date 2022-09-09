using Pulumi.Kubernetes.Types.Inputs.Apps.V1;
using Deployment = Pulumi.Kubernetes.Apps.V1.Deployment;

namespace SimCube.PulumiDeployments.Resources.Kubernetes;

/// <inheritdoc />
public sealed class DeploymentResource : ComponentResource
{
    private const string DefaultLabel = nameof(DefaultLabel);

    private DeploymentResource(NamespaceResource @namespace, string name, DeploymentConfiguration deploymentConfiguration, List<EnvVarArgs>? envVariables = null, ComponentResourceOptions? options = null)
        : base(nameof(DeploymentResource), name, options)
    {
        Guard.Against.Null(deploymentConfiguration, nameof(deploymentConfiguration));

        var customResourceOptions = new CustomResourceOptions
        {
            DependsOn = new List<Resource>
            {
                @namespace,
            },
        };

        Deployment = new(
            name,
            new()
            {
                Metadata = new ObjectMetaArgs
                {
                    Namespace = @namespace.NamespaceName,
                    Name = name,
                    Labels =
                    {
                        { DefaultLabel, name },
                    }
                },
                Spec = CreateDeploymentSpec(deploymentConfiguration, name, envVariables)
            }, customResourceOptions);
    }

    public Deployment Deployment { get; }

    public static DeploymentResource Create(NamespaceResource @namespace, string name, DeploymentConfiguration deploymentConfiguration, List<EnvVarArgs>? envVariables = null, ComponentResourceOptions? componentResourceOptions = null)
    {
        Guard.Against.Null(deploymentConfiguration, nameof(deploymentConfiguration));

        var deployment = new DeploymentResource(@namespace, name, deploymentConfiguration, envVariables, componentResourceOptions);

        componentResourceOptions?.DependsOn.Add(deployment.Deployment);

        return deployment;
    }

    private static DeploymentSpecArgs CreateDeploymentSpec(DeploymentConfiguration deploymentConfiguration, string name, List<EnvVarArgs>? envVarArgsList)
        => new()
        {
            Replicas = deploymentConfiguration.Replicas,
            Selector = new LabelSelectorArgs
            {
                MatchLabels =
                {
                    { DefaultLabel, name },
                },
            },
            Template = CreatePodTemplate(deploymentConfiguration, name, envVarArgsList),
        };

    private static PodTemplateSpecArgs CreatePodTemplate(DeploymentConfiguration deploymentConfiguration, string name, List<EnvVarArgs>? envVarArgsList)
        => new()
        {
            Metadata = new ObjectMetaArgs
            {
                Labels =
                {
                    { DefaultLabel, name },
                },
            },
            Spec = new PodSpecArgs
            {
                Containers = new List<ContainerArgs>
                {
                    CreateContainer(deploymentConfiguration, envVarArgsList),
                }
            }
        };

    private static ContainerArgs CreateContainer(DeploymentConfiguration deploymentConfiguration, List<EnvVarArgs>? envVarArgsList)
    {
        var containerArgs = new ContainerArgs
        {
            Image = $"{deploymentConfiguration.Image}:{Environment.GetEnvironmentVariable("IMAGE_BUILD_VERSION") ?? "latest"}",
            Ports = new List<ContainerPortArgs>()
            {
                new()
                {
                    Name = deploymentConfiguration.Ports.Name,
                    Protocol = deploymentConfiguration.Ports.Protocol,
                    ContainerPortValue = deploymentConfiguration.Ports.Number,
                },
            },
            Name = deploymentConfiguration.ServiceName,
        };

        if (envVarArgsList is not null)
        {
            containerArgs.Env = envVarArgsList;
        }

        return containerArgs;
    }
}