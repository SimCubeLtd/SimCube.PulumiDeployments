namespace SimCube.PulumiDeployments.Extensions;

[SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Pulumi deployment, not shippable")]
public static class ConfigurationExtensions
{
    public static T GetFromJson<T>(this Config config, string key)
    {
        Guard.Against.Null(config, nameof(config));
        Guard.Against.Null(config, nameof(key));

        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonStringEnumConverter());
        return config.RequireObject<JsonElement>(key).Deserialize<T>(options)!;
    }

    public static string GetString(this Config config, string key)
    {
        Guard.Against.Null(config, nameof(config));
        Guard.Against.Null(config, nameof(key));

        return config.Require(key);
    }

    /// <summary>
    /// Performs validation of configuration using FluentValidation Synchronously.
    /// </summary>
    /// <param name="type">The configuration instance.</param>
    /// <typeparam name="TObject">The object type to validate.</typeparam>
    /// <typeparam name="TValidator">The type of the validator which derives from <see cref="AbstractValidator{T}"/>.</typeparam>
    public static void Validate<TObject, TValidator>(this TObject type)
        where TObject : class
        where TValidator : AbstractValidator<TObject>
    {
        var validator = Activator.CreateInstance<TValidator>();
        HandleValidationResult(validator, validator.Validate(type), type);
    }

    /// <summary>
    /// Performs validation of configuration using FluentValidation Asynchronously.
    /// </summary>
    /// <param name="type">The configuration instance.</param>
    /// <param name="cancellationToken">The optional cancellation token.</param>
    /// <typeparam name="TObject">The object type to validate.</typeparam>
    /// <typeparam name="TValidator">The type of the validator which derives from <see cref="AbstractValidator{T}"/>.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task ValidateAsync<TObject, TValidator>(this TObject type, CancellationToken cancellationToken = default)
        where TObject : class
        where TValidator : AbstractValidator<TObject>
    {
        var validator = Activator.CreateInstance<TValidator>();
        HandleValidationResult(validator, await validator.ValidateAsync(type, cancellationToken), type);
    }

    private static void HandleValidationResult<TObject>(
        IValidator<TObject> validator,
        ValidationResult validationResult,
        TObject type)
        where TObject : class
    {
        if (validationResult.IsValid)
        {
            return;
        }

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Validation of configuration failed.");

        for (var index = 1; index < validationResult.Errors.Count; index++)
        {
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{index}. {validationResult.Errors[index]}");
        }

        Console.WriteLine(stringBuilder.ToString());
        validator.ValidateAndThrow(type);
    }
}
