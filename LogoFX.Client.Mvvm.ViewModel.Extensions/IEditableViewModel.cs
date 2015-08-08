using System.Threading.Tasks;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public interface IEditableViewModel
    {        
        Task<bool> SaveAsync();

        void Undo();

        bool IsDirty { get; }
        bool HasErrors { get; }
        bool CanCancelChanges { get; set; }
    }
}
