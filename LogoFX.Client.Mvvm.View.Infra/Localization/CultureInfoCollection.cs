using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace LogoFX.Client.Mvvm.View.Infra.Localization
{
    /// <summary>
    /// Represents a dynamic <see cref="CultureInfo"/> collection that provides notifications when items get added, changed, removed,
    /// or when the whole list is refreshed.
    /// </summary>
    public sealed class CultureInfoCollection : ObservableCollection<CultureInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <c>CultureInfoCollection</c> class.
        /// </summary>
        public CultureInfoCollection()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <c>CultureInfoCollection</c> class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="items">The collection from which the elements are copied.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The collection parameter cannot be null.
        /// </exception>
        public CultureInfoCollection(IEnumerable<CultureInfo> items)
            : base(items)
        {
            
        }
    }
}
