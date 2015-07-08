namespace LogoFX.Client.Model.Contracts
{
    public interface ICanBeDirty
    {
        bool IsDirty { get; }
        void ClearDirty();
    }
}
