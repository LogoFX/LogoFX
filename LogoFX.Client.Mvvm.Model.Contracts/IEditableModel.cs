using System.ComponentModel;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents editable model
    /// </summary>
    public interface IEditableModel : IEditableObject, ICanBeDirty, ICanCancelChanges, ICanCommitChanges
    {
        
    }
}