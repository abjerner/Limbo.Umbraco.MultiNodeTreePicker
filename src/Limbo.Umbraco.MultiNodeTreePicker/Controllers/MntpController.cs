using System;
using System.Collections.Generic;
using System.Linq;
using Asp.Versioning;
using Limbo.Umbraco.MultiNodeTreePicker.Api;
using Limbo.Umbraco.MultiNodeTreePicker.Composers;
using Limbo.Umbraco.MultiNodeTreePicker.Converters;
using Limbo.Umbraco.MultiNodeTreePicker.Models.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Api.Common.Attributes;
using Umbraco.Cms.Api.Management.Controllers;
using Umbraco.Cms.Api.Management.Routing;
using Umbraco.Cms.Web.Common.Authorization;

#pragma warning disable 1591

namespace Limbo.Umbraco.MultiNodeTreePicker.Controllers;

/// <summary>
/// Management API controller exposing the converters available for the multinode treepicker.
/// </summary>
[ApiController]
[ApiVersion(MntpApiConstants.Version)]
[MapToApi(MntpApiConstants.ApiName)]
[ApiExplorerSettings(GroupName = MntpApiConstants.ApiName)]
[Authorize(Policy = AuthorizationPolicies.BackOfficeAccess)]
[VersionedApiBackOfficeRoute(MntpApiConstants.Route)]
public class MntpController : ManagementApiControllerBase {

    private static readonly string[] _versionSeparator = [", Version"];

    private readonly MntpTypeConverterCollection _typeConverterCollection;
    private readonly MntpConverterCollection _itemConverterCollection;

    public MntpController(MntpTypeConverterCollection typeConverterCollection, MntpConverterCollection itemConverterCollection) {
        _typeConverterCollection = typeConverterCollection;
        _itemConverterCollection = itemConverterCollection;
    }

    /// <summary>
    /// Returns a list of all registered type converters and item converters.
    /// </summary>
    [HttpGet("converters")]
    [ProducesResponseType(typeof(IEnumerable<MntpConverterModel>), StatusCodes.Status200OK)]
    public IActionResult GetConverters() {

        IEnumerable<MntpConverterModel> converters = _typeConverterCollection
            .Select(Map)
            .Concat(_itemConverterCollection.Select(Map))
            .OrderBy(x => x.Name);

        return Ok(converters);

    }

    private static MntpConverterModel Map(IMntpConverter converter) {

        Type type = converter.GetType();

        return new MntpConverterModel {
            Type = converter.Alias ?? type.FullName ?? type.Name,
            Name = converter.Name,
            Icon = converter.Icon ?? "icon-box",
            Description = type.AssemblyQualifiedName?.Split(_versionSeparator, StringSplitOptions.None)[0] + ".dll",
            Assembly = type.Assembly.FullName
        };

    }

}
