namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public sealed class ResultEventArgs
    {
        public bool Successful { get; private set; }

        public ResultEventArgs(bool successful)
        {
            Successful = successful;
        }
    }
}