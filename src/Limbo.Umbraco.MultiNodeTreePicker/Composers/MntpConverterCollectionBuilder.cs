using Limbo.Umbraco.MultiNodeTreePicker.Converters;

namespace Limbo.Umbraco.MultiNodeTreePicker.Composers
{

    internal sealed class MntpConverterCollectionBuilder : LazyCollectionBuilderBase<MntpConverterCollectionBuilder, MntpConverterCollection, IMntpItemConverter>
    {

        protected override MntpConverterCollectionBuilder This => this;

    }

}