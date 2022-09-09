namespace SimCube.PulumiDeployments.Configuration;

public sealed class IngressConfiguration
{
    public string Hostname { get; init; } = default!;
    public bool Tls { get; init; }
    public string ServiceName { get; init; } = default!;

    public string Path { get; init; } = "/";

    public string PathType { get; init; } = "ImplementationSpecific";
    public int Port { get; init; } = 5000;
    public bool GenerateCertificate { get; init; }
    public string ClusterIssuer { get; init; } = default!;
    public string SecretName { get; init; } = default!;
    public string IngressClassName { get; set; } = default!;
}