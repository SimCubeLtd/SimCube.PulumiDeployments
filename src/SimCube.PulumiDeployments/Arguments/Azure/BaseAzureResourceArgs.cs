﻿namespace SimCube.PulumiDeployments.Arguments.Azure;

public abstract record BaseAzureResourceArgs
{
    public string ApplicationName { get; init; } = default!;
    public string Environment { get; init; } = default!;
    public string Location { get; init; } = default!;

    public string? SupportAddress { get; init; }

    public ResourceGroupResource ResourceGroup { get; init; } = default!;
}