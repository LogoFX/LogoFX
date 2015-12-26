using System;
using Caliburn.Micro;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents virtual items container.
    /// </summary>
    /// <typeparam name="T">The type of item.</typeparam>
    public sealed partial class VirtualContainer<T> : PropertyChangedBase
        where T : class
    {       
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualContainer{T}"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        public VirtualContainer(int index)
        {
            _index = index;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualContainer{T}"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="model">The model.</param>
        public VirtualContainer(int index, T model)
        {
            _index = index;
            _model = model;
        }

        private readonly int _index;
        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Index
        {
            get { return _index; }
        }

        private T _model;
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        /// <exception cref="System.ArgumentNullException">value;Model cannot be null.</exception>
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