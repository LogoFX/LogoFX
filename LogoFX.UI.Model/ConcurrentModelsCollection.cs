using System.Collections.Generic;
using LogoFX.Core;
using LogoFX.UI.Model.Contracts;

namespace LogoFX.UI.Model
{
    public class ConcurrentModelsCollection<TItem> : IModelsCollection<TItem>
    {
        private readonly ConcurrentObservableCollection<TItem> _items = new ConcurrentObservableCollection<TItem>();
        private ConcurrentObservableCollection<TItem> Items
        {
            get { return _items; }
        }

        IEnumerable<TItem> IReadModelsCollection<TItem>.Items
        {
            get { return Items; }
        }

        public int ItemsCount { get { return Items.Count; } }
        public bool HasItems { get { return ItemsCount > 0; } }
        public void Add(TItem item)
        {
            Items.Add(item);
        }

        public void Remove(TItem item)
        {
            Items.Remove(item);
        }

        public void Update(IEnumerable<TItem> items)
        {
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
}
