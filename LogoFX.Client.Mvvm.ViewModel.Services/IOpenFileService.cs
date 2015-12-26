using System.Collections.Generic;
using System.IO;

namespace LogoFX.Client.Mvvm.ViewModel.Services
{
    /// <summary>
    /// Represents the service that allows opening a file.
    /// </summary>
    public interface IOpenFileService
    {
        /// <summary>
        ///     Gets a <see cref="FileInfo" /> object for the selected file. If multiple files are selected, returns the first selected file.
        /// </summary>
        FileInfo File { get; }

        /// <summary>
        ///     Gets a collection of <see cref="FileInfo" /> objects for the selected files.
        /// </summary>
        IEnumerable<FileInfo> Files { get; }

        /// <summary>
        ///     Gets or sets a filter string that specifies the file types and descriptions to display.
        /// </summary>
        string Filter { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is allows to select multiple files.
        /// </summary>
        bool Multiselect { get; set; }

        /// <summary>
        ///     Gets or sets the initial directory displayed by the file dialog box.
        /// </summary>
        string InitialDirectory { get; set; }

        /// <summary>
        ///     Gets or sets a string shown in the title bar of the file dialog.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        ///     Determines the filename of the file what will be used.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if a file is selected; otherwise <c>false</c>.
        /// </returns>
        bool DetermineFile();
    }
}