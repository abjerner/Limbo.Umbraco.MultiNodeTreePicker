using System;
using Newtonsoft.Json;
using Umbraco.Core.Models.PublishedContent;

namespace Limbo.Umbraco.MultiNodeTreePicker.Converters {
    
    public interface IMntpItemConverter {

        [JsonProperty("name")]
        string Name { get; }

        object Convert(IPublishedPropertyType propertyType, object source);

        Type GetType(IPublishedPropertyType propertyType);

    }

}