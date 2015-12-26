using System.ComponentModel;

namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Represents <c>ViewModel</c>
    /// </summary>
    public interface IViewModel: INotifyPropertyChanged, ISelectable
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>The display name.</value>
        string DisplayName { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expanded.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is expanded; otherwise, <c>false</c>.
        /// </value>
        bool IsExpanded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is current.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is current; otherwise, <c>false</c>.
        /// </value>
        bool IsCurrent { get; set; }

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is selected.
        ///// </summary>
        ///// <value>
        ///// 	<c>true</c> if this instance is selected; otherwise, <c>false</c>.
        ///// </value>
        //bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets the last error.
        /// </summary>
        /// <value>The last error.</value>
        string LastError { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is busy.
        /// </summary>
        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        bool IsBusy { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        IViewModel Parent { get; set; }
    }
}