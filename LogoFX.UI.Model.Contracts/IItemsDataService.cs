namespace LogoFX.UI.Model.Contracts
{
    public interface IItemsDataService<TItem> : IItemsDataProvider<TItem>, IItemsInfoDataProvider,
        IItemsDataManager<TItem>
    {
        
    }
}