namespace LogoFX.Client.Mvvm.Model.Contracts
{
    public interface IMergeable<in T>
    {
        void Merge(T tomerge);
    }
}
