using System;
using System.Collections.Generic;

namespace LogoFX.UI.ViewModels.Services
{
    public interface IReadModelsSelection<out TItem>
    {
        IEnumerable<TItem> Selection { get; }
        event EventHandler SelectionChanged;
    }

    public interface IWriteModelsSelection<in TItem>
    {
        void UpdateSelection(IEnumerable<TItem> newSelection);
    }

    public class ModelsSelection<TItem> : IReadModelsSelection<TItem>, IWriteModelsSelection<TItem>
    {
        public IEnumerable<TItem> Selection { get; private set; }
        public event EventHandler SelectionChanged;
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
