using System.Threading.Tasks;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents view model that is capable of saving and cancelling the associated model changes
    /// </summary>
    public interface IEditableViewModel : IHaveErrors
    {
        /// <summary>
        /// Saves the model changes asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAsync();

        /// <summary>
        /// Cancels the model changes.
        /// </summary>
        void CancelChanges();

        /// <summary>
        /// Gets a value indicating whether the view model is dirty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the view model is dirty; otherwise, <c>false</c>.
        /// </value>
        bool IsDirty { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the changes can be cancelled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the changes can be cancelled; otherwise, <c>false</c>.
        /// </value>
        bool CanCancelChanges { get; set; }
    }    
}
