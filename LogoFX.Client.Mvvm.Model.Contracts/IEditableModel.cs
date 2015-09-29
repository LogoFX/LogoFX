using System.ComponentModel;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    public interface IEditableModel : IEditableObject, ICanBeDirty, IHaveErrors, IDataErrorInfo
    {
        /// <summary>
        /// Cancels all changes and rolls back to the initial object state
        /// </summary>
        void CancelChanges();               

        /// <summary>
        /// Marks object as dirty
        /// </summary>
        void MakeDirty();

        /// <summary>
        /// Sets or gets the value whether changes can be cancelled
        /// </summary>
        bool CanCancelChanges { get; set; }
    }
}