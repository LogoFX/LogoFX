namespace LogoFX.Client.Mvvm.Model.Contracts
{
    public interface IMemento<T>
    {
        IMemento<T> Restore(T target);
    }
}
