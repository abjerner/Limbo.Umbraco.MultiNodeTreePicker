using Skybrud.Umbraco.MultiNodeTreePicker.Converters;
using Umbraco.Cms.Core.Composing;

namespace Skybrud.Umbraco.MultiNodeTreePicker.Composers
{

    internal sealed class MntpConverterCollectionBuilder : LazyCollectionBuilderBase<MntpConverterCollectionBuilder, MntpConverterCollection, IMntpItemConverter>
    {

        protected override MntpConverterCollectionBuilder This => this;

    }

}