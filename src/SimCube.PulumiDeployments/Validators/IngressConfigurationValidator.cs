namespace SimCube.PulumiDeployments.Validators;

public sealed class IngressConfigurationValidator : AbstractValidator<IngressConfiguration>
{
    public IngressConfigurationValidator() =>
        RuleFor(x => x.Hostname)
            .NotNull()
            .NotEmpty();
}
