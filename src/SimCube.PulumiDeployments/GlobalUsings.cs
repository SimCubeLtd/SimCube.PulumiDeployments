// Global using directives

global using System.Diagnostics.CodeAnalysis;
global using System.Globalization;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using Ardalis.GuardClauses;
global using FluentValidation;
global using FluentValidation.Results;
global using Pulumi;
global using Pulumi.Command.Local;
global using Pulumi.Kubernetes.Core.V1;
global using Pulumi.Kubernetes.Networking.V1;
global using Pulumi.Kubernetes.Types.Inputs.Core.V1;
global using Pulumi.Kubernetes.Types.Inputs.Meta.V1;
global using Pulumi.Kubernetes.Types.Inputs.Networking.V1;
global using SimCube.PulumiDeployments.Configuration;
global using SimCube.PulumiDeployments.Extensions;
global using SimCube.PulumiDeployments.Helpers;
global using SimCube.PulumiDeployments.Literals;
global using SimCube.PulumiDeployments.Resources.Helm;
global using SimCube.PulumiDeployments.Resources.Kubernetes;
global using CliWrap;
global using CliWrap.Buffered;