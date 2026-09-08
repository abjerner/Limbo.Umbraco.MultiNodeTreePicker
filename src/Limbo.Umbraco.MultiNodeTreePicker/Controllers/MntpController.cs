using System;
using System.Collections.Generic;
using System.Linq;
using Asp.Versioning;
using Limbo.Umbraco.MultiNodeTreePicker.Api;
using Limbo.Umbraco.MultiNodeTreePicker.Composers;
using Limbo.Umbraco.MultiNodeTreePicker.Converters;
using Limbo.Umbraco.MultiNodeTreePicker.Models.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Api.Common.Attributes;
using Umbraco.Cms.Api.Management.Controllers;
using Umbraco.Cms.Api.Management.Routing;
using Umbraco.Cms.Web.Common.Authorization;

#pragma warning disable 1591

namespace Limbo.Umbraco.MultiNodeTreePicker.Controllers;

[ApiController]
[MapToApi(MntpApiConstants.Alias)]
[Authorize(Policy = AuthorizationPolicies.SectionAccessContent)]
[ApiVersion(MntpApiConstants.Version)]
[ApiExplorerSettings(GroupName = MntpApiConstants.GroupName)]
[VersionedApiBackOfficeRoute(MntpApiConstants.Route)]
public class MntpController : ManagementApiControllerBase {

    private static readonly string[] _versionSeparator = [", Version"];

    private readonly MntpTypeConverterCollection _typeConverterCollection;
    private readonly MntpConverterCollection _itemConverterCollection;

    public MntpController(MntpTypeConverterCollection typeConverterCollection, MntpConverterCollection itemConverterCollection) {
        _typeConverterCollection = typeConverterCollection;
        _itemConverterCollection = itemConverterCollection;
    }

    [HttpGet("converters")]
    public IEnumerable<MntpConverter> GetTypes() {
        return _typeConverterCollection.ToArray().Select(Map).Union(_itemConverterCollection.ToArray().Select(Map));
    }

    private static MntpConverter Map(IMntpTypeConverter converter) {

        Type type = converter.GetType();

        MntpConverter model = new() {
            Type = converter.Alias,
            Icon = $"{converter.Icon ?? "icon-box"} color-{type.Assembly.FullName?.Split('.')[0].ToLower()}",
            Name = converter.Name,
            Description = type.AssemblyQualifiedName?.Split(_versionSeparator, StringSplitOptions.None)[0] + ".dll",
            Assembly = type.Assembly.FullName
        };

        return model;

    }

    private static MntpConverter Map(IMntpItemConverter converter) {

        Type type = converter.GetType();

        MntpConverter model = new() {
            Type = converter.Alias,
            Icon = $"{converter.Icon ?? "icon-box"} color-{type.Assembly.FullName?.Split('.')[0].ToLower()}",
            Name = converter.Name,
            Description = type.AssemblyQualifiedName?.Split(_versionSeparator, StringSplitOptions.None)[0] + ".dll",
            Assembly = type.Assembly.FullName
        };

        return model;

    }

}