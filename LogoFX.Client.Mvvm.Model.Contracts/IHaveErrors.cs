namespace LogoFX.Client.Mvvm.Model.Contracts
{
    public interface IHaveErrors
    {
        bool HasErrors { get; }

        void SetError(string error, string propertyName);

        void ClearError(string propertyName);
    }
}