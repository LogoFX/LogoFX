using System.Windows.Input;
using Caliburn.Micro;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{    
    public abstract partial class PagingScreenViewModel<TItem, TModel>
    {
        /// <summary>
        /// Represents a screen with support for paging and filtering specified type object view models.
        /// </summary>
        /// <typeparam name="TFilterModel">The type of the filter model.</typeparam>
        public abstract class WithFilter<TFilterModel> : PagingScreenViewModel<TItem, TModel>
            where TFilterModel : class, IFilterModel
        {
            #region Fields

            private TFilterModel _emptyFilter;

            #endregion

            #region Commands

            private ICommand _clearFilterCommand;
            /// <summary>
            /// Gets the clear filter command.
            /// </summary>
            /// <value>
            /// The clear filter command.
            /// </value>
            public ICommand ClearFilterCommand
            {
                get
                {
                    return _clearFilterCommand ??
                           (_clearFilterCommand = ActionCommand
                               .When(() => FilteredCount != Count)
                               .Do(() =>
                               {
                                   ClearFilter();
                                   RefreshData();
                               })
                               .RequeryOnPropertyChanged(this, () => FilteredCount)
                               .RequeryOnPropertyChanged(this, () => Count));
                }
            }

            #endregion

            #region Public Properties

            private FilterViewModelBase<TFilterModel> _filter;
            /// <summary>
            /// Gets or sets the filter.
            /// </summary>
            /// <value>
            /// The filter.
            /// </value>
            public FilterViewModelBase<TFilterModel> Filter
            {
                get { return _filter; }
                protected set
                {
                    if (_filter == value)
                    {
                        return;
                    }

                    if (_filter != null)
                    {
                        _filter.Filter -= OnFilter;
                        ScreenExtensions.TryDeactivate(_filter, true);
                    }

                    _filter = value;

                    if (_filter != null)
                    {
                        if (_emptyFilter == null)
                        {
                            _emptyFilter = _filter.Model == null ? null : (TFilterModel)_filter.Model.Clone();
                        }
                        ScreenExtensions.TryActivate(_filter);
                        _filter.Filter += OnFilter;

                    }

                    NotifyOfPropertyChange();
                }
            }

            #endregion

            #region Virtual Methods

            /// <summary>
            /// Clears the filter.
            /// </summary>
            protected virtual void ClearFilter()
            {
                if (Filter == null)
                {
                    return;
                }

                Filter.SetModel(_emptyFilter == null ? null : (TFilterModel)_emptyFilter.Clone());
            }

            #endregion

            #region Private Members

            private void FilterData()
            {
                RefreshData();
            }

            private void OnFilter(object sender, FilterEventArgs<TFilterModel> e)
            {
                FilterData();
                NotifyOfPropertyChange(() => FilteredCount);
            }

            #endregion
        }
    }
}
