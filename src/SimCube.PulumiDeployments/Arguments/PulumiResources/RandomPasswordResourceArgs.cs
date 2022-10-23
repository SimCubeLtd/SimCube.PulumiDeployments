namespace SimCube.PulumiDeployments.Arguments.PulumiResources;

public sealed record RandomPasswordResourceArgs(
    string Name,
    int Length = 16,
    int MinLower = 1,
    int MinUpper = 1,
    int MinNumeric = 1,
    int MinSpecial = 1);