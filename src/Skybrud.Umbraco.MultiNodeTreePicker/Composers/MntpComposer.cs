using Skybrud.Umbraco.MultiNodeTreePicker.Converters;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Skybrud.Umbraco.MultiNodeTreePicker.Composers {

    [RuntimeLevel(MinLevel = RuntimeLevel.Boot)]
    internal sealed class MntpComposer : IUserComposer  {
        
        public void Compose(Composition composition) {

            composition.RegisterUnique<MntpConverterCollection>();

            composition
                .MntpConverters()
                .Add(() => composition.TypeLoader.GetTypes<IMntpItemConverter>())
                ;

        }

    }

}