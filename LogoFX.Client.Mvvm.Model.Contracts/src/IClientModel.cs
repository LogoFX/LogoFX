using System;
using System.ComponentModel;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents client model
    /// </summary>
    public interface IClientModel : INotifyPropertyChanged
#if NET45
          , IDataErrorInfo
#endif
    {
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        int GetHashCode();
    }

    /// <summary>
    /// Represents value object
    /// </summary>
    public interface IValueObject
    {
        
    }

    /// <summary>
    /// Represents service
    /// </summary>
    public interface IService
    {
        
    }

    /// <summary>
    /// Represents entity with identifier.
    /// </summary>
    /// <typeparam name="TEntityId">The type of the entity identifier.</typeparam>
    public interface IEntity<TEntityId>
    {
        /// <summary>
        /// Gets the entity identifier.
        /// </summary>
        /// <value>
        /// The entity identifier.
        /// </value>
        IEntityId<TEntityId> EntityId { get; }
    }

    /// <summary>
    /// Represents entity identifier
    /// </summary>
    /// <typeparam name="TEntityId">The type of the entity identifier.</typeparam>
    public interface IEntityId<TEntityId>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        TEntityId Id { get; set; }
    }

    /// <summary>
    /// Represents an object which can manage dirty state.
    /// </summary>
    public interface IIsDirty
    {
        /// <summary>
        /// Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is dirty; otherwise, <c>false</c>.
        /// </value>
        bool IsDirty { get; }

        /// <summary>
        /// Raised when dirty state is changed.
        /// </summary>
        event EventHandler IsDirtyChanged;

        /// <summary>
        /// Clears the dirty state.
        /// </summary>
        void ClearDirty();
    }   

    /// <summary>
    /// Represents editable client model
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public interface IEditableClientModel<TId> : IEntity<TId>, IIsDirty, IUndoRedo, IEditableObject where TId : IEquatable<TId>
    {
        
    }
}
