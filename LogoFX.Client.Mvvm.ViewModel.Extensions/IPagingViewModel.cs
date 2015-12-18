namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public interface IPagingViewModel
    {
        int TotalPages { get; set; }

        double PageLeft { get; set; }

        double PageWidth { get; set; }

        int CurrentPage { get; set; }

        void RestoreSelection();
    }
}
