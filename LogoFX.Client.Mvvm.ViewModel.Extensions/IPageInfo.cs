using System.Windows;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public interface IPageInfo
    {
        int PageCount { get; }

        int CurrentPage { get; set; }

        Rect CurrentPageRect { get; }

        void BeginUpdate();

        void EndUpdate();
    }
}