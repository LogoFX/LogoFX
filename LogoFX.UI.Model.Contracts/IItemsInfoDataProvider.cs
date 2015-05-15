namespace LogoFX.UI.Model.Contracts
{
    public interface IItemsInfoDataProvider
    {
        int ItemsCount { get; }
        bool HasItems { get; }
    }
}