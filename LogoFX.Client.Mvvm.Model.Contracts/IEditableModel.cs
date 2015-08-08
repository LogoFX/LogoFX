using System.ComponentModel;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    public interface IEditableModel : IEditableObject, ICanBeDirty, IHaveErrors
    {
        void CancelChanges();               

        void MakeDirty();

        bool CanCancelChanges { get; set; }
    }
}