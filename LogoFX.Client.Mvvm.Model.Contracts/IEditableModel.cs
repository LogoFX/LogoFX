using System.ComponentModel;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    public interface IEditableModel : IEditableObject, ICanBeDirty
    {
        void Undo();

        void Apply();

        void CancelApply();

        bool HasErrors { get; }

        void MakeDirty();

        bool CanUndo { get; set; }
    }
}