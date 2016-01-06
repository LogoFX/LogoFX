using System;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// Stack with capacity, bottom items beyond the capacity are discarded.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class RoundStack<T>
    {
        private readonly T[] _items;

        // top == bottom ==> full
        private int _top = 1;
        private int _bottom = 0;

        /// <summary>
        /// Returns true if the <see cref="RoundStack&lt;T&gt;"/> is full; otherwise, returns false.
        /// </summary>
        public bool IsFull
        {
            get
            {
                return _top == _bottom;
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="RoundStack&lt;T&gt;"/>.
        /// </summary>
        public int Count
        {
            get
            {
                int count = _top - _bottom - 1;
                if (count < 0)
                {
                    count += _items.Length;
                }                    
                return count;
            }
        }

        /// <summary>
        /// Gets the capacity of the <see cref="RoundStack&lt;T&gt;"/>.
        /// </summary>
        public int Capacity
        {
            get
            {
                return _items.Length - 1;
            }
        }

        /// <summary>
        /// Creates <see cref="RoundStack&lt;T&gt;"/> with given capacity
        /// </summary>
        /// <param name="capacity"></param>
        public RoundStack(int capacity)
        {
            if (capacity < 1)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity need to be at least 1");
            _items = new T[capacity + 1];
        }

        /// <summary>
        /// Removes and returns the object at the top of the <see cref="RoundStack&lt;T&gt;"/>.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            if (Count > 0)
            {
                T removed = _items[_top];
                _items[_top--] = default(T);
                if (_top < 0)
                {
                    _top += _items.Length;
                }                    
                return removed;
            }
            else
                throw new InvalidOperationException("Cannot pop from emtpy stack");
        }

        /// <summary>
        /// Inserts an object at the top of the <see cref="RoundStack&lt;T&gt;"/>.
        /// </summary>
        /// <param name="item"></param>
        public void Push(T item)
        {
            if (IsFull)
            {
                _bottom++;
                if (_bottom >= _items.Length)
                {
                    _bottom -= _items.Length;
                }                    
            }
            if (++_top >= _items.Length)
            {
                _top -= _items.Length;
            }                
            _items[_top] = item;
        }

        /// <summary>
        /// Returns the object at the top of the <see cref="RoundStack&lt;T&gt;"/> without removing it.
        /// </summary>
        public T Peek()
        {
            return _items[_top];
        }

        /// <summary>
        /// Removes all the objects from the <see cref="RoundStack&lt;T&gt;"/>.
        /// </summary>
        public void Clear()
        {
            if (Count > 0)
            {
                for (int i = 0; i < _items.Length; i++)
                {
                    _items[i] = default(T);
                }
                _top = 1;
                _bottom = 0;
            }
        }
    }
}
