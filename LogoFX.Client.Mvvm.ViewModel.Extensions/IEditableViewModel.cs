using System.Threading.Tasks;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public interface IEditableViewModel
    {        
        Task<bool> SaveAsync();

        void CancelChanges();

        bool IsDirty { get; }
        bool HasErrors { get; }
        bool CanCancelChanges { get; set; }
    }
}
