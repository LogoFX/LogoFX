namespace LogoFX.Client.Model.Contracts
{
    public interface IMergeable<in T>
    {
        void Merge(T tomerge);
    }
}
