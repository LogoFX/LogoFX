using System.ComponentModel;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    public interface IEditableModel : IEditableObject, ICanBeDirty, IHaveErrors
    {
        void RejectChanges();               

        void MakeDirty();

        bool CanRejectChanges { get; set; }
    }
}