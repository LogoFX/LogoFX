namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents models collection
    /// </summary>
    /// <typeparam name="TItem">Type of model</typeparam>
    public interface IModelsCollection<TItem> : IReadModelsCollection<TItem>, IInfoModelsCollection,
        IWriteModelsCollection<TItem>
    {
        
    }
}