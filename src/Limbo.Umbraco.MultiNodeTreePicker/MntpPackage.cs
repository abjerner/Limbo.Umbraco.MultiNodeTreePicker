using System;
using System.Diagnostics;
using Umbraco.Cms.Core.Semver;

namespace Limbo.Umbraco.MultiNodeTreePicker;

/// <summary>
/// Static class with various information and constants about the package.
/// </summary>
public static class MntpPackage {

    /// <summary>
    /// Gets the alias of the package.
    /// </summary>
    public const string Alias = "Limbo.Umbraco.MultiNodeTreePicker";

    /// <summary>
    /// Gets the friendly name of the package.
    /// </summary>
    public const string Name = "Limbo Multinode Treepicker";

    /// <summary>
    /// Gets the version of the package.
    /// </summary>
    public static readonly Version Version = typeof(MntpPackage).Assembly.GetName().Version!;

    /// <summary>
    /// Gets the informational version of the package.
    /// </summary>
    public static readonly string InformationalVersion = FileVersionInfo
        .GetVersionInfo(typeof(MntpPackage).Assembly.Location).ProductVersion!
        .Split('+')[0];

    /// <summary>
    /// Gets the semantic version of the package.
    /// </summary>
    public static readonly SemVersion SemVersion = InformationalVersion;

}