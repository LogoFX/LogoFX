using System.Windows.Input;
using Caliburn.Micro;
using LogoFX.Client.Mvvm.Commanding;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{    
    public abstract partial class PagingScreenViewModel<TItem, TModel>
    {
        public abstract class WithFilter<TFilterModel> : PagingScreenViewModel<TItem, TModel>
            where TFilterModel : class, IFilterModel
        {
            #region Fields

            private TFilterModel _emptyFilter;

            private FilterViewModelBase<TFilterModel> _filter;

            private ICommand _clearFilterCommand;

            #endregion

            #region Commands

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
