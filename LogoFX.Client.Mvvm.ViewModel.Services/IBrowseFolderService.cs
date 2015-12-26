using System;
using System.IO;

namespace LogoFX.Client.Mvvm.ViewModel.Services
{
    /// <summary>
    /// Represents the service that allows browsing for folder.
    /// </summary>
    public interface IBrowseFolderService
    {
        /// <summary>
        /// Gets or sets a value indicating whether the New Folder button appears in the folder browser dialog box.
        /// </summary>
        bool ShowNewFolderButton { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DirectoryInfo"/> object for the path selected by the user.
        /// </summary>
        DirectoryInfo SelectedPath { get; set; }

        /// <summary>
        /// Gets or sets the root folder where the browsing starts from.
        /// </summary>
        Environment.SpecialFolder RootFolder { get; set; }

        /// <summary>
        /// Gets or sets the descriptive text displayed above the tree view control in the dialog box.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Determines the name of the folder what will be used.
        /// </summary>
        /// <returns>
        /// <c>true</c> if a file is selected; otherwise <c>false</c>.
        /// </returns>
        bool DetermineFolder();
    }
}