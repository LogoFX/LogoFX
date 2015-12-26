using System;
using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.ViewModel.Services
{
    /// <summary>
    /// Represents an object with read-only selection properties.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public interface IReadModelsSelection<out TItem>
    {
        /// <summary>
        /// Gets the selection.
        /// </summary>
        /// <value>
        /// The selection.
        /// </value>
        IEnumerable<TItem> Selection { get; }

        /// <summary>
        /// Occurs when selection is changed.
        /// </summary>
        event EventHandler SelectionChanged;
    }

    /// <summary>
    /// Represents an object which enables changing selection.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public interface IWriteModelsSelection<in TItem>
    {
        /// <summary>
        /// Updates the selection.
        /// </summary>
        /// <param name="newSelection">The new selection.</param>
        void UpdateSelection(IEnumerable<TItem> newSelection);
    }

    /// <summary>
    /// Represents models selection.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public class ModelsSelection<TItem> : IReadModelsSelection<TItem>, IWriteModelsSelection<TItem>
    {
        /// <summary>
        /// Gets the selection.
        /// </summary>
        /// <value>
        /// The selection.
        /// </value>
        public IEnumerable<TItem> Selection { get; private set; }


        /// <summary>
        /// Occurs when selection is changed.
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Updates the selection.
        /// </summary>
        /// <param name="newSelection">The new selection.</param>
        public void UpdateSelection(IEnumerable<TItem> newSelection)
        {
            Selection = newSelection;
            if (SelectionChanged != null)
            {
                SelectionChanged(this, new EventArgs());
            }
        }
    }
}
