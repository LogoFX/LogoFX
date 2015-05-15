namespace LogoFX.UI.Model.Contracts
{
    public interface IMergeable<in T>
    {
        void Merge(T tomerge);
    }
}
