namespace SimCube.PulumiDeployments.Resources.Kubernetes;

/// <inheritdoc />
public sealed class IngressResource : ComponentResource
{
    private IngressResource(NamespaceResource @namespace, string name, IngressConfiguration[] ingressConfiguration, string? serviceName, ComponentResourceOptions? options = null)
        : base(nameof(IngressResource), name, options)
    {
        Guard.Against.Null(ingressConfiguration, nameof(ingressConfiguration));

        var customResourceOptions = new CustomResourceOptions
        {
            DependsOn = new List<Resource>
            {
                @namespace,
            },
        };

        var ingressRules = CreateIngressRulesFromConfig(ingressConfiguration, serviceName);
        var defaultBackend = CreateDefaultBackend(ingressConfiguration, serviceName);
        var tlsRules = CreateTlsRules(ingressConfiguration);

        var ingressSpec = new IngressSpecArgs
        {
            DefaultBackend = defaultBackend,
            Rules = ingressRules,
        };

        if (tlsRules.Any())
        {
            ingressSpec.Tls = tlsRules;
        }

        Ingress = new(
            name,
            new()
            {
                Metadata = new ObjectMetaArgs
                {
                    Namespace = @namespace.NamespaceName,
                    Name = name,
                    Annotations = CreateAnnotations(ingressConfiguration)
                },
                Spec = ingressSpec,
            }, customResourceOptions);
    }

    public Ingress Ingress { get; }

    public static IngressResource Create(NamespaceResource @namespace, string name, IngressConfiguration[] ingressConfiguration,
        string? serviceName = null, ComponentResourceOptions? componentResourceOptions = null)
    {
        Guard.Against.Null(ingressConfiguration, nameof(ingressConfiguration));
        Guard.Against.Null(@namespace, nameof(@namespace));

        var ingress = new IngressResource(@namespace, name, ingressConfiguration, serviceName, componentResourceOptions);

        componentResourceOptions?.DependsOn.Add(ingress.Ingress);

        return ingress;
    }

    private static IngressRuleArgs[] CreateIngressRulesFromConfig(IEnumerable<IngressConfiguration> ingressConfiguration, string? serviceName) =>
        ingressConfiguration.Select(x => new IngressRuleArgs
        {
            Host = x.Hostname,
            Http = new HTTPIngressRuleValueArgs
            {
                Paths = new List<HTTPIngressPathArgs>
                {
                    new()
                    {
                        Path = x.Path,
                        PathType = x.PathType,
                        Backend = new IngressBackendArgs
                        {
                            Service = new IngressServiceBackendArgs
                            {
                                Name = serviceName ?? x.ServiceName,
                                Port = new ServiceBackendPortArgs
                                {
                                    Number = x.Port,
                                }
                            }
                        }
                    }
                }
            }
        }).ToArray();

    private static IngressBackendArgs CreateDefaultBackend(IReadOnlyList<IngressConfiguration> ingressConfiguration, string? serviceName)
        => new()
        {
            Service = new IngressServiceBackendArgs
            {
                Name = serviceName ?? ingressConfiguration[0].ServiceName,
                Port = new ServiceBackendPortArgs
                {
                    Number = ingressConfiguration[0].Port,
                }
            }
        };

    private static IngressTLSArgs[] CreateTlsRules(IEnumerable<IngressConfiguration> ingressConfiguration)
        => ingressConfiguration
            .Where(x => x.Tls)
            .Select(x => new IngressTLSArgs
            {
                Hosts = new List<string>
                {
                    x.Hostname,
                }
            }).ToArray();

    private static Dictionary<string, string> CreateAnnotations(IReadOnlyList<IngressConfiguration> ingressConfiguration)
    {
        var annotations = new Dictionary<string, string>
        {
            [KubernetesLiterals.IngressClassAnnotation] = ingressConfiguration[0].IngressClassName
        };

        if (ingressConfiguration.Any(x => x.GenerateCertificate))
        {
            annotations[KubernetesLiterals.ClusterIssuerAnnotation] = ingressConfiguration[0].ClusterIssuer;
        }

        return annotations;
    }
}