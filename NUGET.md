[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md) [![NuGet](https://img.shields.io/nuget/vpre/Limbo.Umbraco.MultiNodeTreePicker.svg)](https://www.nuget.org/packages/Limbo.Umbraco.MultiNodeTreePicker) [![NuGet](https://img.shields.io/nuget/dt/Limbo.Umbraco.MultiNodeTreePicker.svg)](https://www.nuget.org/packages/Limbo.Umbraco.MultiNodeTreePicker) [![Umbraco Marketplace](https://img.shields.io/badge/umbraco-marketplace-%233544B1)](https://marketplace.umbraco.com/package/limbo.umbraco.multinodetreepicker)

**Limbo.Umbraco.MultiNodeTreePicker** adds a special multinode treepicker to the Umbraco backoffice in which developers can select a custom item converter.

The purpose of an item converter is to control the C# type returned by the `.Value()` method or the corresponding property in a ModelsBuilder generated model. This is particular useful in a SPA/Headless Umbraco implementation, where the ModelsBuilder model can then be returned directly via a WebAPI endpoint.

### Changelog

The [**releases page**][Releases] lists all releases, and each there will be some information for each release on the most significant changes.

### Documentation

For examples on how to use the package, see our [**documentation**][Documentation].

[Documentation]: https://packages.limbo.works/163af59b
[NuGetPackage]: https://www.nuget.org/packages/Limbo.Umbraco.MultiNodeTreePicker
[GitHubRelease]: https://github.com/abjerner/Limbo.Umbraco.MultiNodeTreePicker/releases/latest
[Releases]: https://github.com/abjerner/Limbo.Umbraco.MultiNodeTreePicker/releases