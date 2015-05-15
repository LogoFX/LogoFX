namespace LogoFX.UI.Model.Contracts
{
    public interface ICanBeDirty
    {
        bool IsDirty { get; }
        void ClearDirty();
    }
}
