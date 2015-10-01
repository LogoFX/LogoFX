using System.Collections.Generic;
using LogoFX.Client.Mvvm.Model.Contracts;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// Thread-safe models collection
    /// </summary>
    /// <typeparam name="TItem">Type of model</typeparam>
    public class ConcurrentModelsCollection<TItem> : ModelsCollectionBase, IModelsCollection<TItem>, IWriteRangeModelsCollection<TItem>
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

        public override int ItemsCount { get { return Items.Count; } }
        public override bool HasItems { get { return ItemsCount > 0; } }       

        public void Add(TItem item)
        {            
            Items.Add(item);
            SafeRaiseHasItemsChanged();
        }        

        public void Remove(TItem item)
        {
            Items.Remove(item);
            SafeRaiseHasItemsChanged();
        }

        public void Update(IEnumerable<TItem> items)
        {
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
            SafeRaiseHasItemsChanged();
        }

        public void Clear()
        {
            Items.Clear();
            SafeRaiseHasItemsChanged();
        }

        public void AddRange(IEnumerable<TItem> items)
        {
            Items.AddRange(items);
            SafeRaiseHasItemsChanged();
        }

        public void RemoveRange(IEnumerable<TItem> items)
        {
            Items.RemoveRange(items);
            SafeRaiseHasItemsChanged();
        }
    }
}
