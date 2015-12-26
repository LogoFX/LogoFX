using System;
using System.ComponentModel;
using System.Threading.Tasks;
using LogoFX.Client.Mvvm.Model.Contracts;
using LogoFX.Client.Mvvm.ViewModel.Interfaces;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents an editable object view model that is an item in the paged collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EditablePagingItemViewModel<T> : EditableObjectViewModel<T>, IPagingItem
        where T : IEditableModel, IHaveErrors, IDataErrorInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditablePagingItemViewModel{T}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="isNew">if set to <c>true</c> [is new].</param>
        protected EditablePagingItemViewModel(T model, bool isNew)
            : base(model)
        {
            IsNew = isNew;
        }

        private bool _isNew;
        /// <summary>
        /// Gets a value indicating whether this instance is new.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is new; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Gets or sets the on saving function.
        /// </summary>
        /// <value>
        /// The on saving function.
        /// </value>
        public Func<T, bool> OnSavingFunc { get; set; }

        /// <summary>
        /// Override this method to provide custom save changes logic.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        protected override Task<bool> SaveMethod(T model)
        {
            return Task.Run(() => OnSavingFunc(model));
        }
    }
}