namespace LogoFX.Client.Mvvm.Model.Contracts
{
    public interface IModelsCollection<TItem> : IReadModelsCollection<TItem>, IInfoModelsCollection,
        IWriteModelsCollection<TItem>
    {
        
    }
}