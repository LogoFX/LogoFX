using System;
using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StackWithNotifications<T>
    {
        private readonly Stack<T> _internalStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="StackWithNotifications{T}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public StackWithNotifications(int capacity)
        {
            _internalStorage = new Stack<T>(capacity);
        }

        /// <summary>
        /// Occurs when stack contents are changed.
        /// </summary>
        public event EventHandler StackChanged;

        /// <summary>
        /// Gets the number of items.
        /// </summary>
        /// <value>
        /// The number of items.
        /// </value>
        public int Count
        {
            get { return _internalStorage.Count; }
        }

        /// <summary>
        /// Pushes the specified item on top of the stack.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Push(T item)
        {
            _internalStorage.Push(item);
            RaiseStackChanged();
        }

        /// <summary>
        /// Pops an item from the top of the stack.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            var item = _internalStorage.Pop();
            RaiseStackChanged();
            return item;
        }

        /// <summary>
        /// Returns an item from the top of the stack without removing it.
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            return _internalStorage.Peek();
        }

        /// <summary>
        /// Clears the contents of the stack.
        /// </summary>
        public void Clear()
        {
            _internalStorage.Clear();
            RaiseStackChanged();
        }

        private void RaiseStackChanged()
        {
            if (StackChanged != null)
            {
                StackChanged(this, new EventArgs());
            }
        }
    }
}