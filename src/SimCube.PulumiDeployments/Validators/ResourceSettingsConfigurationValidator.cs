namespace SimCube.PulumiDeployments.Validators;

public sealed class ResourceSettingsConfigurationValidator : AbstractValidator<ResourceConfiguration>
{
    public ResourceSettingsConfigurationValidator()
    {
        RuleFor(x => x.CpuRequest)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.MemoryRequest)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.CpuLimit)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.MemoryLimit)
            .NotNull()
            .NotEmpty();
    }
}
