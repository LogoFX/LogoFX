using System.ComponentModel;

namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// SelectionChangingEventArgs
    /// </summary>
    public class SelectionChangingEventArgs:CancelEventArgs
    {
        private readonly object _item;
        private readonly bool _isSelecting;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionChangingEventArgs"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="isSelecting">if set to <c>true</c> is selecting.</param>
        public SelectionChangingEventArgs(object item,bool isSelecting)
        {
            _item = item;
            _isSelecting = isSelecting;
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <value>The item.</value>
        public object Item
        {
            get { return _item; }
        }

        /// <summary>
        /// Gets a value indicating whether this operation is selecting operation.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this operation is selecting; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelecting
        {
            get { return _isSelecting; }
        }
    }
}
