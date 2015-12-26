namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Item that can be current
    /// </summary>
    public interface ICurrent
    {
        /// <summary>
        /// If current
        /// </summary>
        bool IsCurrent { get; set; }
    }
}