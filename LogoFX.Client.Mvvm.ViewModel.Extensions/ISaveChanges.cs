using System.Threading.Tasks;
using LogoFX.Client.Mvvm.ViewModel.Shared;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents an object that allows to react to different save changes outcomes.
    /// </summary>
    public interface ISaveChanges
    {
        /// <summary>
        /// Displays the save changes prompt and captures the selected prompt option.
        /// </summary>
        /// <returns></returns>
        Task<MessageResult> OnSaveChangesPrompt();

        /// <summary>
        /// Reacts to the save changes error.
        /// </summary>
        /// <returns></returns>
        Task OnSaveChangesWithErrors();
    }
}