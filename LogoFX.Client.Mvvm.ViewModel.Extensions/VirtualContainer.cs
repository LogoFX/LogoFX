using System;
using Caliburn.Micro;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public sealed partial class VirtualContainer<T> : PropertyChangedBase
        where T : class
    {
        private readonly int _index;

        public VirtualContainer(int index)
        {
            _index = index;
        }

        public VirtualContainer(int index, T model)
        {
            _index = index;
            _model = model;
        }

        public int Index
        {
            get { return _index; }
        }

        private T _model;

        public T Model
        {
            get { return _model; }
            set
            {
                if (Equals(_model, value))
                {
                    return;
                }

                if (Equals(value, null))
                {
                    throw new ArgumentNullException("value", "Model cannot be null.");
                }

                _model = value;
                NotifyOfPropertyChange();
            }
        }
    }
}