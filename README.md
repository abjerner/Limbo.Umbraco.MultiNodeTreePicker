# Limbo Multinode Treepicker

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/abjerner/Limbo.Umbraco.MultiNodeTreePicker/blob/v1/main/LICENSE.md)
[![NuGet](https://img.shields.io/nuget/vpre/Limbo.Umbraco.MultiNodeTreePicker.svg)](https://www.nuget.org/packages/Limbo.Umbraco.MultiNodeTreePicker)
[![NuGet](https://img.shields.io/nuget/dt/Limbo.Umbraco.MultiNodeTreePicker.svg)](https://www.nuget.org/packages/Limbo.Umbraco.MultiNodeTreePicker)
[![Limbo.Umbraco.MultiNodeTreePicker at packages.limbo.works](https://img.shields.io/badge/limbo-packages-blue)](https://packages.limbo.works/limbo.umbraco.multinodetreepicker/)
[![Umbraco Marketplace](https://img.shields.io/badge/umbraco-marketplace-%233544B1)](https://marketplace.umbraco.com/package/limbo.umbraco.multinodetreepicker)
<!--[![Our Umbraco](https://img.shields.io/badge/our-umbraco-%233544B1)](https://our.umbraco.com/packages/backoffice-extensions/limbo-multinode-treepicker/)-->

**Limbo.Umbraco.MultiNodeTreePicker** adds a special multinode treepicker to the Umbraco backoffice in which developers can select a custom item converter.

The purpose of an item converter is to control the C# type returned by the `.Value()` method or the corresponding property in a ModelsBuilder generated model. This is particular useful in a SPA/Headless Umbraco implementation, where the ModelsBuilder model can then be returned directly via a WebAPI endpoint.

<table>
  <tr>
    <td><strong>License:</strong></td>
    <td><a href="https://github.com/abjerner/Limbo.Umbraco.MultiNodeTreePicker/blob/v1/main/LICENSE.md"><strong>MIT License</strong></a></td>
  </tr>
  <tr>
    <td><strong>Umbraco:</strong></td>
    <td>Umbraco 10, 11 and 12</td>
  </tr>
  <tr>
    <td><strong>Target Framework:</strong></td>
    <td>.NET 6</td>
  </tr>
</table>







<br /><br />

## Installation

The package targets Umbraco 10 and is only available via [**NuGet**][NuGetPackage]. To install the package, you can use either .NET CLI

```
dotnet add package Limbo.Umbraco.MultiNodeTreePicker --version 1.0.6
```

or the NuGet Package Manager:

```
Install-Package Limbo.Umbraco.MultiNodeTreePicker -Version 1.0.6
```

> **Note**  
> This package replaces our older [**Skybrud.Umbraco.MultiNodeTreePicker**](https://github.com/abjerner/Skybrud.Umbraco.MultiNodeTreePicker) package. See this package for older versions of Umbraco.

[NuGetPackage]: https://www.nuget.org/packages/Limbo.Umbraco.MultiNodeTreePicker
[GitHubRelease]: https://github.com/abjerner/Limbo.Umbraco.MultiNodeTreePicker/releases









<br /><br />

## Examples

At [**@limbo-works**](https://github.com/limbo-works) we typically use Umbraco as a headless CMS, and being able to control the generated models therefore makes a lot of sense. If a given page has some related content, it doesn't make sense for us to return the full model of a related model, so we instead have an item class with the needed properties - this class could be called `TestItem`.

Normally the property with the related content would return the full model for each page, but with the special multinode treepicker from this package, we can implement a custom item converter.

We can do this by implementing the [`IMntpItemConverter`](https://github.com/abjerner/Skybrud.Umbraco.MultiNodeTreePicker/blob/master/src/Skybrud.Umbraco.MultiNodeTreePicker/Converters/IMntpItemConverter.cs) interface, but to get going a bit quicker, the package also contains the abstract [`MntpGenericItemConverter`](https://github.com/abjerner/Skybrud.Umbraco.MultiNodeTreePicker/blob/master/src/Skybrud.Umbraco.MultiNodeTreePicker/Converters/MntpGenericItemConverter.cs) class we can use instead:

```csharp
public class TestMntpItemConverter : MntpGenericItemConverter<TestItem> {

    public TestMntpItemConverter() : base("Default item converter", x => new TestItem(x)) { }

}
```

```csharp
public class TestItem {
        
        public Guid Key { get; }

        public string Name { get; }

        public string Url { get; }
        
        public TestItem(IPublishedContent content) {
            Key = content.Key;
            Name = content.Name;
            Url = content.Url;
        }

    }
```

The `MntpGenericItemConverter` class requires us to specify a name for the converter, and then a callback function that will be used for converting each `IPublishedContent` to the desired type.

![image](https://user-images.githubusercontent.com/3634580/90198696-b2271d80-ddd2-11ea-8ac8-dd9f59a513f2.png)

When the data type is configured to use our item converter (see screenshot above), properties of this type will now return `List<TestItem>` instead of `List<IPublishedContent>`.

With this special multinode treepicker, it's the datatype of the individual property, that determines the returned value. Another property could for instance be for selecting contact persons where we'd need a bit more information than what's available from the `TestItem` class, so we can create another item converter:

```csharp
  public class TestMntpPersonItemConverter : MntpGenericItemConverter<TestPersonItem> {

      public TestMntpPersonItemConverter() : base("Person item converter", x => new TestPersonItem(x)) { }

  }
```

```csharp
  public class TestPersonItem : TestItem {

      public string Phone { get; }

      public string Email { get; }

      public TestPersonItem(IPublishedContent content) : base(content) {
          Phone = content.Value<string>("phone");
          Email = content.Value<string>("email");
      }

  }
```

![image](https://user-images.githubusercontent.com/3634580/90199149-3037f400-ddd4-11ea-93c0-ce7661e04531.png)







<br /><br />

## Documentation

- [See more at **packages.limbo.works**](https://packages.limbo.works/limbo.umbraco.multinodetreepicker/)



<br /><br />

## Inspiration

The item converters in this package was inspired by a similar concept in the [Contentment](https://github.com/leekelleher/umbraco-contentment) package. Thanks for creating and sharing this with us [**@leekelleher**](https://github.com/leekelleher) 👍
