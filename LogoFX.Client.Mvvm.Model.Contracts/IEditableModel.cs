using System.ComponentModel;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    public interface IEditableModel : IEditableObject, ICanBeDirty, ICanCancelChanges
    {
        
    }
}