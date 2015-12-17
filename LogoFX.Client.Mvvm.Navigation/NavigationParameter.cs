namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Navigation parameter. Implement to provide custom navigation behavior
    /// </summary>
    public abstract class NavigationParameter
    {
        public abstract void Navigate();
    }
}