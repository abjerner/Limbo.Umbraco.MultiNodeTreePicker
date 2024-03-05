using Limbo.Umbraco.MultiNodeTreePicker.Converters;
using Umbraco.Cms.Core.Composing;

namespace Limbo.Umbraco.MultiNodeTreePicker.Composers;

internal sealed class MntpTypeConverterCollectionBuilder : LazyCollectionBuilderBase<MntpTypeConverterCollectionBuilder, MntpTypeConverterCollection, IMntpTypeConverter> {

    protected override MntpTypeConverterCollectionBuilder This => this;

}