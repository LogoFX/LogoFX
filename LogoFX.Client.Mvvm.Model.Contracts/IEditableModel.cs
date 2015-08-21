using System.ComponentModel;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    public interface IEditableModel : IEditableObject, ICanBeDirty, IHaveErrors, IDataErrorInfo
    {
        void CancelChanges();               

        void MakeDirty();

        bool CanCancelChanges { get; set; }
    }
}