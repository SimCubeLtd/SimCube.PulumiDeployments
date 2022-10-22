using SimCube.PulumiDeployments.Configuration.Azure;

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

    /// <summary>
    /// Gets the Azure Active directory description.
    /// </summary>
    /// <param name="configuration">The configuration instance <see cref="BaseAzureConfiguration"/>.</param>
    /// <returns>The description as a string.</returns>
    public static string GetAzureActiveDirectoryDescription(
        this BaseAzureConfiguration configuration)
    {
        Guard.Against.Null(configuration, nameof(configuration));

        return string.Join(Environment.NewLine, configuration.GetAzureActiveDirectoryTags());
    }

    /// <summary>
    /// Gets the Azure Active directory tags.
    /// </summary>
    /// <param name="configuration">The configuration instance <see cref="BaseAzureConfiguration"/>.</param>
    /// <returns>The Tags as a list of type string.</returns>
    [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "Pulumi requires List")]
    public static List<string> GetAzureActiveDirectoryTags(
        this BaseAzureConfiguration configuration)
    {
        Guard.Against.Null(configuration, nameof(configuration));

        return configuration.GetTags("Azure Active Directory").Select(x => $"{x.Key}={x.Value}").ToList();
    }

    /// <summary>
    /// Gets the Azure Active directory tags.
    /// </summary>
    /// <param name="configuration">The configuration instance <see cref="BaseAzureConfiguration"/>.</param>
    /// <param name="location">The location as a string.</param>
    /// <returns>A dictionary containing all the tags on the resource.</returns>
    public static Dictionary<string, string> GetTags(
        this BaseAzureConfiguration configuration,
        string location)
    {
        Guard.Against.Null(configuration, nameof(configuration));

        return new()
        {
            {
                TagName.Application, configuration.ApplicationName
            },
            {
                TagName.Environment, configuration.Environment
            },
            {
                TagName.Location, location
            },
        };
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