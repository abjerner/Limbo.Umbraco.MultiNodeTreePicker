using System;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.MultiNodeTreePicker.Converters
{

    public interface IMntpItemConverter : IPropertyValueConverter
    {

        [JsonProperty("name")]
        string Name { get; }

        object Convert(IPublishedPropertyType propertyType, object source);

        Type GetType(IPublishedPropertyType propertyType);

    }

}