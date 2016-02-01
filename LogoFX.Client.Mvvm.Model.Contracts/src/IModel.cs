using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents model with basic support for property notifications and built-in Id
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IModel<T> : 
        INotifyPropertyChanged, 
        IHaveId<T>,
#if NET45
         IDataErrorInfo,
#endif        
        INotifyDataErrorInfo,
        IHaveErrors,
        IHaveExternalErrors
        where T:IEquatable<T>
    {
        /// <summary>
        /// Model name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Model description
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        [Obsolete]        
        bool IsReadOnly { get;}

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        [Obsolete]
        IEnumerable<IPropertyData> Properties { get; }
    }

    /// <summary>
    /// Represents model with <see cref="int"/> as identifier.
    /// </summary>
    public interface IModel: IModel<int>
    {
    }
}