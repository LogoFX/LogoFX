namespace LogoFX.Client.Mvvm.ViewModel.Interfaces.Services
{
    public interface IModelsCollection<TItem> : IReadModelsCollection<TItem>, IInfoModelsCollection,
        IWriteModelsCollection<TItem>
    {
        
    }
}