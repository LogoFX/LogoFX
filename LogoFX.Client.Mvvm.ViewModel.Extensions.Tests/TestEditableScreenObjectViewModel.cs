﻿using System.Threading.Tasks;
using Caliburn.Micro;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Client.Mvvm.ViewModel.Shared;
using Solid.Practices.Scheduling;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Tests
{
    class TestConductorViewModel : Conductor<object>.Collection.OneActive
    {
        
    }

    class TestEditableScreenSimpleObjectViewModel : EditableScreenObjectViewModel<SimpleEditableModel>
    {
        private readonly IMessageService _messageService;
        private readonly TaskFactory _taskFactory = TaskFactoryFactory.CreateTaskFactory();

        public TestEditableScreenSimpleObjectViewModel(
            IMessageService messageService,             
            SimpleEditableModel model) : base(model)
        {
            _messageService = messageService;
        }

        internal bool WasCancelingChangesCalled { get; private set; }

        protected override Task<bool> SaveMethod(SimpleEditableModel model)
        {            
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

        protected override async Task OnChangesCanceling()
        {
            await _taskFactory.StartNew(() =>
            {
                WasCancelingChangesCalled = true;
            });
        }
    }

    class TestEditableScreenCompositeObjectViewModel : EditableScreenObjectViewModel<CompositeEditableModel>
    {
        private readonly IMessageService _messageService;
        private readonly TaskFactory _taskFactory = TaskFactoryFactory.CreateTaskFactory();

        public TestEditableScreenCompositeObjectViewModel(
            IMessageService messageService,
            CompositeEditableModel model) : base(model)
        {
            _messageService = messageService;
        }

        internal bool WasCancelingChangesCalled { get; private set; }

        protected override Task<bool> SaveMethod(CompositeEditableModel model)
        {
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

        protected override async Task OnChangesCanceling()
        {
            await _taskFactory.StartNew(() =>
            {
                WasCancelingChangesCalled = true;
            });
        }
    }
}