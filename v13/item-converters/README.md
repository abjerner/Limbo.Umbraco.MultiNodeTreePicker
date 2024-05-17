# Item Converters

The package introduces a concept called *item converters*, which essentially is about converting the default multinode treepicker return values into something else.

For instance, if you have a picker on a page for selecting related pages, you most likely don't need the `IPublishedContent`, but rather some limited information about each selected page (or item if you will).

When you have created a new MNTP data type, you can select an item converter. Currently the package only ships with a single item converter, but by implementing the `IMntpItemConverter` interface, you can create your own custom item converter.

You can select the item converter via the options on the multinode treepicker data type.

![image](https://github.com/abjerner/Limbo.Umbraco.MultiNodeTreePicker/assets/3634580/72a6dd79-4b70-4df0-bd06-baf1e954d514)

In the example below, the item converter converts the selected `IPublishedContent` into instances of `MyContentItem`. As also show in this example, your custom item converters can use dependency injection - e.g. for delegating the work of creating the item to an item factory.

When implementing the `IMntpItemConverter` interface, the `Convert` method is responsible for convert each `IPublishedContent` to the desired value, whereas the `GetType` is used for indication a common type for each item.

```csharp
using System;
using MntpDemo.Factories;
using MntpDemo.Models.Common;
using Limbo.Umbraco.MultiNodeTreePicker.Converters;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace MntpDemo.Converters;

public class MyContentItemConverter : IMntpItemConverter {

    private readonly MyItemFactory _itemFactory;

    #region Properties

    public string Name => "My Content Item Converter";

    public string Icon => "icon-box-open";

    #endregion

    #region Constructors

    public CitiContentItemConverter(MyItemFactory itemFactory) {
        _itemFactory = itemFactory;
    }

    #endregion

    #region Member methods

    public object? Convert(IPublishedPropertyType propertyType, IPublishedContent? source) {
        return source is null ? null : _itemFactory.CreateContentItem(source);
    }

    public Type GetType(IPublishedPropertyType propertyType) {
        return typeof(MyContentItem);
    }

    #endregion

}
```

Under the hood, the package will automatically detect whether the data type is configured as a multi picker or a single picker, which you don't have to worry about here.

However as illustrated in the exampel above, the `IMntpItemConverter` interface  only lets you control how each item is converted, but doesn't let you control the overall return value - e.g. if you wish to return something else than `IReadOnlyList<MyContentItem>`.

The package also contains the `IMntpTypeConverter` interface, which gives you additional control of the conversion.