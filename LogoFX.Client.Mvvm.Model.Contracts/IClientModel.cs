using System;
using System.ComponentModel;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    public interface IClientModel : INotifyPropertyChanged, IDataErrorInfo
    {
        int GetHashCode();
    }

    public interface IValueObject : IClientModel
    {
        
    }

    public interface IEntity<TId> : IHaveId<TId> where TId : IEquatable<TId>
    {
        
    }

    public interface IIsDirty
    {
        bool IsDirty { get; }
        event EventHandler IsDirtyChanged;
        void ClearDirty();
    }

    public interface IRevertible
    {
        bool CanUndo { get; }
        bool CanRedo { get; }
        void Undo();
        void Redo();
    }

    public interface IEditableClientModel<TId> : IEntity<TId>, IIsDirty, IRevertible, IEditableObject where TId : IEquatable<TId>
    {
        
    }
}
