namespace LogoFX.Client.Mvvm.Model.Contracts
{
    public interface ICanBeDirty
    {
        bool IsDirty { get; }
        void ClearDirty();
    }
}
