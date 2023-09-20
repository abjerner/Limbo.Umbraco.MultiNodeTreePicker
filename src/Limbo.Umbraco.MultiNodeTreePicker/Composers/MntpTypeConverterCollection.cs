using Limbo.Umbraco.MultiNodeTreePicker.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core.Composing;

namespace Limbo.Umbraco.MultiNodeTreePicker.Composers {

    /// <summary>
    /// Class representing a collection of <see cref="IMntpTypeConverter"/>.
    /// </summary>
    public sealed class MntpTypeConverterCollection : BuilderCollectionBase<IMntpTypeConverter> {

        private readonly Dictionary<string, IMntpTypeConverter> _lookup;

        /// <summary>
        /// Initializes an new instance based on the specified <paramref name="converters"/>.
        /// </summary>
        /// <param name="converters">The item converters that should make up the collection.</param>
        public MntpTypeConverterCollection(Func<IEnumerable<IMntpTypeConverter>> converters) : base(converters) {

            _lookup = new Dictionary<string, IMntpTypeConverter>(StringComparer.OrdinalIgnoreCase);

            foreach (IMntpTypeConverter item in this) {

                string? typeName = MntpUtils.GetTypeAlias(item.GetType());
                if (typeName != null && _lookup.ContainsKey(typeName) == false) {
                    _lookup.Add(typeName, item);
                }

            }

        }

        /// <summary>
        /// Gets the item converter associated with the specified <paramref name="typeName"/>.
        /// </summary>
        /// <param name="typeName">The name of the type.</param>
        /// <param name="result">When this method returns, contains the item converter associated with the specified
        /// <paramref name="typeName"/>, if the key is found; otherwise <c>null</c>. This parameter is passed
        /// uninitialized.</param>
        /// <returns><c>true</c> if the collection contains an item converter with the specified
        /// <paramref name="typeName"/>; otherwise, false.</returns>
        public bool TryGet(string typeName, [NotNullWhen(true)] out IMntpTypeConverter? result) {
            return _lookup.TryGetValue(typeName, out result);
        }

    }

}