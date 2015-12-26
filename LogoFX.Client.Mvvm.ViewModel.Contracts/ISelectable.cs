namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Represents an object that can be selected.
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Selection status
        /// </summary>
        bool IsSelected { get; set; }

        ///// <summary>
        ///// Selection event
        ///// </summary>
        //event EventHandler Selected;
        ///// <summary>
        ///// Un-selection event
        ///// </summary>
        //event EventHandler UnSelected;
    }
}   