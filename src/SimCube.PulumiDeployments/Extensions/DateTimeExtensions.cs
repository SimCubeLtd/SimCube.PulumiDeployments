namespace SimCube.PulumiDeployments.Extensions;

public static class DateTimeExtensions
{
    /// <summary>
    /// Converts a DateTime to Rfc3339.
    /// </summary>
    /// <param name="value">The DateTime to convert.</param>
    /// <returns>The DateTime represented in Rfc3339.</returns>
    public static string ToRfc3339String(
        this DateTime value) => XmlConvert.ToString(value, XmlDateTimeSerializationMode.Utc);
}