using Limbo.Umbraco.MultiNodeTreePicker.Converters;

namespace Limbo.Umbraco.MultiNodeTreePicker.Composers
{

    [RuntimeLevel(MinLevel = RuntimeLevel.Boot)]
    internal sealed class MntpComposer : IUserComposer
    {

        public void Compose(Composition composition)
        {

            composition.RegisterUnique<MntpConverterCollection>();

            composition
                .MntpConverters()
                .Add(() => composition.TypeLoader.GetTypes<IMntpItemConverter>())
                ;

        }

    }

}