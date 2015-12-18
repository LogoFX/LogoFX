using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public sealed partial class VirtualContainer<T>
    {
        public sealed class Collection : IList<VirtualContainer<T>>, IList
        {
            #region Nested Types

            private sealed class Segment : IEnumerable<VirtualContainer<T>>
            {
                #region Fields

                private const int DefaultTimeToLive = 10;

                private int _timeToLive = DefaultTimeToLive;
                private readonly int _startIndex;
                private readonly List<VirtualContainer<T>> _data;

                #endregion

                #region Constructors

                private Segment(int startIndex, IEnumerable<VirtualContainer<T>> data)
                {
                    _startIndex = startIndex;
                    _data = new List<VirtualContainer<T>>(data);
                }

                //public Segment(int startIndex, IEnumerable<T> data)
                //    : this(startIndex, data.Select((x, i) => new VirtualContainer<T>(i + startIndex, x)))
                //{
                //}

                public Segment(int startIndex, int size)
                    : this(startIndex, Enumerable.Range(0, size).Select(i => new VirtualContainer<T>(i + startIndex)))
                {
                }

                #endregion

                #region Public Properties

                public VirtualContainer<T> this[int index]
                {
                    get { return _data[index - _startIndex]; }
                }

                public int StartIndex { get { return _startIndex; } }

                public int Count { get { return _data.Count; } }

                #endregion

                #region Public Methods

                public bool DecreaseTimeToLive()
                {
                    return --_timeToLive > 0;
                }

                public void ResetTimeToLive()
                {
                    _timeToLive = DefaultTimeToLive;
                }

                #endregion

                #region IEnumerable

                public IEnumerator<VirtualContainer<T>> GetEnumerator()
                {
                    return _data.GetEnumerator();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }

                #endregion
            }

            private sealed class VirtualEnumerator : IEnumerator<VirtualContainer<T>>
            {
                #region Fields

                private int _currentIndex;

                private readonly IList<VirtualContainer<T>> _list;

                #endregion

                #region Constructors

                public VirtualEnumerator(IList<VirtualContainer<T>> list)
                {
                    _list = list;
                    Reset();
                }

                #endregion

                #region IEnumerator<T>

                public void Dispose()
                {

                }

                public bool MoveNext()
                {
                    ++_currentIndex;
                    if (_currentIndex >= _list.Count)
                    {
                        return false;
                    }

                    return true;
                }

                public void Reset()
                {
                    _currentIndex = -1;
                }

                public VirtualContainer<T> Current
                {
                    get { return _list[_currentIndex]; }
                }

                object IEnumerator.Current
                {
                    get { return Current; }
                }

                #endregion
            }

            #endregion

            #region Fields

            private readonly object _syncRoot = new object();

            private readonly IDictionary<int, Segment> _cache =
                new Dictionary<int, Segment>();

            private readonly int _count;

            // startIndex, count, result collection.
            private readonly Func<int, int, Task<IEnumerable<T>>> _getItemsFunc;
            private readonly int _segmentSize;

            private int _lastSegmentIndex = -1;
            private Segment _lastSegment;

            #endregion

            #region Constructors

            public Collection(int count, Func<int, int, Task<IEnumerable<T>>> getItemsFunc, int segmentSize = 256)
            {
                _count = count;
                _getItemsFunc = getItemsFunc;
                _segmentSize = segmentSize;
            }

            #endregion

            #region Public Properties

            public IEnumerable<VirtualContainer<T>> CachedCollection
            {
                get { return _cache.Values.SelectMany(x => x).ToList(); }
            }

            #endregion

            #region Private Members

            private int GetSegmentIndex(int itemIndex)
            {
                return itemIndex / _segmentSize;
            }

            private int GetStartItemIndex(int segmentIndex)
            {
                return segmentIndex * _segmentSize;
            }

            private Segment GetSegment(int segmentIndex)
            {
                if (segmentIndex == _lastSegmentIndex)
                {
                    return _lastSegment;
                }

                Segment segment;

                if (!_cache.TryGetValue(segmentIndex, out segment))
                {
                    foreach (var segmentPair in _cache.ToList())
                    {
                        if (!segmentPair.Value.DecreaseTimeToLive())
                        {
                            _cache.Remove(segmentPair.Key);
                        }
                    }

                    segment = CreateSegment(segmentIndex);
                    _cache.Add(segmentIndex, segment);
                }
                else
                {
                    segment.ResetTimeToLive();
                }

                _lastSegment = segment;
                _lastSegmentIndex = segmentIndex;
                return segment;
            }

            private Segment CreateSegment(int segmentIndex)
            {
                int startIndex = GetStartItemIndex(segmentIndex);
                var segment = new Segment(startIndex, _segmentSize);
                LoadDataAsync(segment);
                return segment;
            }

            private async void LoadDataAsync(Segment segment)
            {
                int startIndex = segment.StartIndex;
                int count = segment.Count;

                IEnumerable<T> items = await _getItemsFunc(startIndex, count);

                int index = startIndex;
                foreach (T item in items)
                {
                    segment[index].Model = item;
                    ++index;
                }
            }

            #endregion

            #region IList<T>

            public IEnumerator<VirtualContainer<T>> GetEnumerator()
            {
                return new VirtualEnumerator(this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            void ICollection<VirtualContainer<T>>.Add(VirtualContainer<T> item)
            {
                throw new NotImplementedException();
            }

            void ICollection<VirtualContainer<T>>.Clear()
            {
                throw new NotImplementedException();
            }

            bool ICollection<VirtualContainer<T>>.Contains(VirtualContainer<T> item)
            {
                throw new NotImplementedException();
            }

            void ICollection<VirtualContainer<T>>.CopyTo(VirtualContainer<T>[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            bool ICollection<VirtualContainer<T>>.Remove(VirtualContainer<T> item)
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { return _count; }
            }

            bool ICollection<VirtualContainer<T>>.IsReadOnly
            {
                get { return true; }
            }

            int IList<VirtualContainer<T>>.IndexOf(VirtualContainer<T> item)
            {
                return -1;
            }

            void IList<VirtualContainer<T>>.Insert(int index, VirtualContainer<T> item)
            {
                throw new NotImplementedException();
            }

            void IList<VirtualContainer<T>>.RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            public VirtualContainer<T> this[int index]
            {
                get
                {
                    int segmentIndex = GetSegmentIndex(index);
                    Segment segment = GetSegment(segmentIndex);
                    VirtualContainer<T> current = segment[index];
                    return current;
                }

                set { throw new NotImplementedException(); }
            }

            #endregion

            #region IList

            int IList.Add(object value)
            {
                throw new NotImplementedException();
            }

            void IList.Clear()
            {
                throw new NotImplementedException();
            }

            bool IList.Contains(object value)
            {
                throw new NotImplementedException();
            }

            int IList.IndexOf(object value)
            {
                return -1;
            }

            void IList.Insert(int index, object value)
            {
                throw new NotImplementedException();
            }

            bool IList.IsFixedSize
            {
                get { return true; }
            }

            bool IList.IsReadOnly
            {
                get { return true; }
            }

            void IList.Remove(object value)
            {
                throw new NotImplementedException();
            }

            void IList.RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            object IList.this[int index]
            {
                get { return this[index]; }
                set { throw new NotImplementedException(); }
            }

            #endregion

            #region ICollection

            void ICollection.CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            int ICollection.Count
            {
                get { return Count; }
            }

            bool ICollection.IsSynchronized
            {
                get { return false; }
            }

            object ICollection.SyncRoot
            {
                get { return _syncRoot; }
            }

            #endregion
        }
    }
}
