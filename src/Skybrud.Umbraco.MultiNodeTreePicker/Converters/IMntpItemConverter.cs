using System;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Skybrud.Umbraco.MultiNodeTreePicker.Converters
{

    public interface IMntpItemConverter
    {

        [JsonProperty("name")]
        string Name { get; }

        object Convert(IPublishedPropertyType propertyType, object source);

        Type GetType(IPublishedPropertyType propertyType);

    }

}