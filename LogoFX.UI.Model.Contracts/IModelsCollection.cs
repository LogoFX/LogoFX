namespace LogoFX.UI.Model.Contracts
{
    public interface IModelsCollection<TItem> : IReadModelsCollection<TItem>, IInfoModelsCollection,
        IWriteModelsCollection<TItem>
    {
        
    }
}