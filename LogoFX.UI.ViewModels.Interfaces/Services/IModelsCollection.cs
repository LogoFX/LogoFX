namespace LogoFX.UI.ViewModels.Interfaces.Services
{
    public interface IModelsCollection<TItem> : IReadModelsCollection<TItem>, IInfoModelsCollection,
        IWriteModelsCollection<TItem>
    {
        
    }
}