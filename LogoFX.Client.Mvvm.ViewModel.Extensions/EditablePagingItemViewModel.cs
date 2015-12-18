using System;
using System.Threading.Tasks;
using LogoFX.Client.Mvvm.Model.Contracts;
using LogoFX.Client.Mvvm.ViewModel.Interfaces;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public abstract class EditablePagingItemViewModel<T> : EditableObjectViewModel<T>, IPagingItemViewModel
        where T : IEditableModel, IHaveErrors
    {
        protected EditablePagingItemViewModel(T model, bool isNew)
            : base(model)
        {
            IsNew = isNew;
        }

        private bool _isNew;

        public bool IsNew
        {
            get { return _isNew; }
            internal set
            {
                if (_isNew == value)
                {
                    return;
                }

                _isNew = value;
                NotifyOfPropertyChange();
            }
        }

        bool ISelectable.IsSelected
        {
            get { return IsSelected; }
            set { IsSelected = value; }
        }

        public Func<T, bool> OnSavingFunc { get; set; }

        protected override Task<bool> SaveMethod(T model)
        {
            return Task.Run(() => OnSavingFunc(model));
        }
    }
}