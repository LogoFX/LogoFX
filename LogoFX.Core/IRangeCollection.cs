using System.Collections.Generic;

namespace LogoFX.Core
{
    /// <summary>
    /// Represents collection which is able to add and remove 
    /// range of items as single operation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRangeCollection<T>
    {
        /// <summary>
        /// Adds range of items as single operation.
        /// </summary>
        /// <param name="range"></param>
        void AddRange(IEnumerable<T> range);

        /// <summary>
        /// Removes the range of items as single operation.
        /// </summary>
        /// <param name="range">The range.</param>
        void RemoveRange(IEnumerable<T> range);
    }
}
