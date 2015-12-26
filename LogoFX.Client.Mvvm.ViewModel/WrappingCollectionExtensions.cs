using System;
using System.Collections;
#if !WinRT && !SILVERLIGHT
using System.Windows.Data;
#endif

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Contains extensions for <see cref="WrappingCollection"/>
    /// </summary>
    public static class WrappingCollectionExtensions
    {
        /// <summary>
        /// Creates the view on the specified <see cref="WrappingCollection"/>.
        /// </summary>
        /// <param name="collection">The specified <see cref="WrappingCollection"/></param>
        /// <returns></returns>
        public static ListCollectionView AsView(this WrappingCollection collection)
        {
            return new ListCollectionView(collection.AsList());
        }

        /// <summary>
        /// Sets the specified filter on the collection view.
        /// </summary>
        /// <param name="collectionView">The collection view.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static ListCollectionView WithFiltering(this ListCollectionView collectionView, Predicate<object> filter) 
        {
            collectionView.Filter = filter;
            return collectionView;
        }

        /// <summary>
        /// Sets the specified filter on the collection view.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionView">The collection view.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static ListCollectionView WithFiltering<T>(this ListCollectionView collectionView, Predicate<T> filter) 
            where T:class            
        {
            collectionView.Filter = a => filter(a as T);
            return collectionView;
        }

        /// <summary>
        /// Orders the specified collection view using the specified comparer.
        /// </summary>
        /// <param name="collectionView">The collection view.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public static ListCollectionView OrderBy(this ListCollectionView collectionView, IComparer comparer)
        {
            collectionView.CustomSort = comparer;
            return collectionView;
        }

        /// <summary>
        /// Assigns the specified data source to the specified <see cref="WrappingCollection"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The specified collection.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// collection
        /// or
        /// source
        /// </exception>
        public static T WithSource<T>(this T collection, IEnumerable source) where T : WrappingCollection
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            if (source == null)
                throw new ArgumentNullException("source");

            collection.AddSource(source);

            return collection;
        }

        /// <summary>
        /// Groups the specified collection view according to the specified property name.
        /// </summary>
        /// <param name="collectionView">The collection view.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static ListCollectionView WithGrouping(this ListCollectionView collectionView, string propertyName)
        {
            if (collectionView.GroupDescriptions != null && collectionView.CanGroup && !string.IsNullOrWhiteSpace(propertyName))
                collectionView.GroupDescriptions.Add(new PropertyGroupDescription(propertyName));
            return collectionView;
        }
    }
}