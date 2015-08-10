using System.Threading.Tasks;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Tests
{
    class TestEditableScreenObjectViewModel : EditableScreenObjectViewModel<SimpleEditableModel>
    {
        private readonly IMessageService _messageService;

        public TestEditableScreenObjectViewModel(IMessageService messageService, SimpleEditableModel model) : base(model)
        {
            _messageService = messageService;
        }

        protected override Task<bool> SaveMethod(SimpleEditableModel model)
        {
            model.ClearDirty();
            return Task.FromResult(true);
        }

        protected override Task<MessageResult> OnSaveChangesPrompt()
        {
            return _messageService.ShowAsync("Save changes?", DisplayName, MessageButton.YesNoCancel,
                MessageImage.Question);
        }

        protected override Task OnSaveChangesWithErrors()
        {
            return _messageService.ShowAsync("Cannot save error changes.", DisplayName, MessageButton.OK, MessageImage.Warning);
        }        
    }
}