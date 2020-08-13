using System;
using System.Collections;
using System.Linq;

// ReSharper disable PossibleNullReferenceException

namespace Skybrud.Umbraco.MultiNodeTreePicker {
    
    internal static class EnumerableExtensions {

        // TODO: Move logic to Skybrud.Essentials

        public static IEnumerable Cast(this IEnumerable source, Type targetType) {
            return (IEnumerable) typeof(Enumerable)
                .GetMethod("Cast")
                .MakeGenericMethod(targetType)
                .Invoke(null, new object[] { source });
        }

        public static IList ToList(this IEnumerable source, Type targetType) {
            return (IList) typeof(Enumerable)
                .GetMethod("ToList")
                .MakeGenericMethod(targetType)
                .Invoke(null, new object[] { source });
        }

    }

}