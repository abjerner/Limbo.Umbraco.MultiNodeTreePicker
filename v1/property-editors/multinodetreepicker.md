# Multinode Treepicker

Alias: `Limbo.Umbraco.MultiNodeTreePicker`
Name: Limbo Multinode Treepicker

The package contains a single property editor, that extends Umbraco's default Multinode Treeepicker property editor, and works in a similar way, but with some additional functionality.

## Data Type

Since the property editor works like the one in Umbraco, most of the options in the data type are inherited from Umbraco.

### Item Converter

`Limbo.Umbraco.MultiNodeTreePicker` adds an option for selecting an item converter. By default, the multinode treepicker will return the selected items as `IEnumerable<IPublishedContent>` (or `IPublishedContent` if a single picker).

The item converter is a custom C# class that let's you convert the `IPublishedContent` instances to something else.