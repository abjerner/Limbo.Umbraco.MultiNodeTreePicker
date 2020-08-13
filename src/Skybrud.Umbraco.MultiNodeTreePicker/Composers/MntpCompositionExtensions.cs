using Umbraco.Core.Composing;

namespace Skybrud.Umbraco.MultiNodeTreePicker.Composers {
    
    internal static class MntpCompositionExtensions {
        
        public static MntpConverterCollectionBuilder MntpConverters(this Composition composition) {
            return composition.WithCollectionBuilder<MntpConverterCollectionBuilder>();
        }

    }

}