namespace SimCube.PulumiDeployments.Resources.Kubernetes;

public sealed class PersistentVolumeClaimResource : ComponentResource
{
    private PersistentVolumeClaimResource(
        NamespaceResource @namespace,
        string claimName,
        string volumeSize,
        string storageClassName,
        bool isReadWriteOnce = true,
        ComponentResourceOptions? options = null)
        : base(nameof(PersistentVolumeClaimResource), claimName, options)
    {
        Guard.Against.Null(@namespace, nameof(@namespace));

        var customResourceOptions = new CustomResourceOptions
        {
            DependsOn = new List<Resource>
            {
                @namespace,
            },
        };

        _ = new PersistentVolumeClaim(
            claimName,
            new()
            {
                Metadata = new ObjectMetaArgs
                {
                    Name = claimName,
                    Namespace = @namespace.NamespaceName,
                },
                Spec = new PersistentVolumeClaimSpecArgs
                {
                    StorageClassName = storageClassName,
                    AccessModes = new()
                    {
                        isReadWriteOnce ?
                            KubernetesLiterals.PvcAccessModeReadWriteOnce :
                            KubernetesLiterals.PvcAccessModeReadWriteMany,
                    },
                    Resources = new ResourceRequirementsArgs
                    {
                        Requests = new()
                        {
                            [KubernetesLiterals.PvcAccessResourceRequestStorage] = volumeSize,
                        },
                    },
                },
            },
            customResourceOptions);

        RegisterOutputs();
    }

    public static void CreatePvc(
        NamespaceResource @namespace,
        string claimName,
        string volumeSize,
        string storageClassName,
        bool isReadWriteOnce = true,
        ComponentResourceOptions? options = null)
    {
        Guard.Against.Null(@namespace, nameof(@namespace));

        _ = new PersistentVolumeClaimResource(
            @namespace,
            claimName,
            volumeSize,
            storageClassName,
            isReadWriteOnce,
            options);
    }
}
