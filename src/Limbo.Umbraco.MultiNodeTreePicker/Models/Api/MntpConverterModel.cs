namespace Limbo.Umbraco.MultiNodeTreePicker.Models.Api;

/// <summary>
/// API model describing a converter available for selection in the backoffice.
/// </summary>
public class MntpConverterModel {

    /// <summary>
    /// Gets the alias of the converter type - eg. <c>MyProject.Converters.MyConverter, MyProject</c>.
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// Gets the friendly name of the converter.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the icon of the converter.
    /// </summary>
    public required string Icon { get; init; }

    /// <summary>
    /// Gets a description of the converter - by default the name of the assembly the converter lives in.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Gets the full name of the assembly of the converter.
    /// </summary>
    public string? Assembly { get; init; }

}
