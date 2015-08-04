using System.ComponentModel;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    public interface IEditableModel : IEditableObject, ICanBeDirty, IHaveErrors
    {
        void Undo();               

        void MakeDirty();

        bool CanUndo { get; set; }
    }
}