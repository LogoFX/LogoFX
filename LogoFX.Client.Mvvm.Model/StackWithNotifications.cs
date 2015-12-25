using System;
using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.Model
{
    public class StackWithNotifications<T>
    {
        private readonly Stack<T> _internalStorage;

        public StackWithNotifications(int capacity)
        {
            _internalStorage = new Stack<T>(capacity);
        }

        public event EventHandler StackChanged;        

        public int Count
        {
            get { return _internalStorage.Count; }
        }

        public void Push(T item)
        {
            _internalStorage.Push(item);
            RaiseStackChanged();
        }

        public T Pop()
        {
            var item = _internalStorage.Pop();
            RaiseStackChanged();
            return item;
        }

        public T Peek()
        {
            return _internalStorage.Peek();
        }

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