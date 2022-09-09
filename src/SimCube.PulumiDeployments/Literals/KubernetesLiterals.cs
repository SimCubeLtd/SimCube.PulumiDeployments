namespace SimCube.PulumiDeployments.Literals;

public static class KubernetesLiterals
{
    public const string IngressClassAnnotation = "kubernetes.io/ingress.class";
    public const string ClusterIssuerAnnotation = "cert-manager.io/cluster-issuer";
    public const string TlsSecretType = "kubernetes.io/tls";
    public const string ForceSslRedirectAnnotation = "ingress.kubernetes.io/force-ssl-redirect";
    public const string PvcAccessModeReadWriteOnce = "ReadWriteOnce";
    public const string PvcAccessModeReadWriteMany = "ReadWriteMany";
    public const string PvcAccessResourceRequestStorage = "storage";
    public const string DefaultSelectorKey = "app";
}