using System.Threading.Tasks;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public interface IEditableViewModel : IHaveErrors
    {        
        Task<bool> SaveAsync();

        void CancelChanges();

        bool IsDirty { get; }        
        bool CanCancelChanges { get; set; }
    }    
}
