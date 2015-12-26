using System.Threading.Tasks;

namespace LogoFX.Client.Mvvm.ViewModel.Interfaces
{
    public interface IEditableViewModel
    {
        bool IsDirty { get; }
        bool HasErrors { get; }
        bool CanUndo { get; set; }
        
        void Undo();
        Task<bool> SaveAsync();        
    }
}
