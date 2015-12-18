using System;
using LogoFX.Client.Mvvm.ViewModel.Interfaces;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public interface IWithSelection<out TItem> : IHaveSelectedItem<TItem>, IHaveSelectedItems<TItem>
    {
        Action<object, SelectionChangingEventArgs> SelectionHandler { get; set; }                
    }
}