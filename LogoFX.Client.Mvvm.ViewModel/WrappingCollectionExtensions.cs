using System;
using System.Collections;
#if !WinRT && !SILVERLIGHT
using System.Windows.Data;
#endif

namespace LogoFX.Client.Mvvm.ViewModel
{
    public static class WrappingCollectionExtensions
    {
        public static ListCollectionView AsView(this WrappingCollection collection)
        {
            return new ListCollectionView(collection.AsList());
        }

        public static ListCollectionView WithFiltering(this ListCollectionView collectionView, Predicate<object> filter) 
        {
            collectionView.Filter = filter;
            return collectionView;
        }

        public static ListCollectionView WithFiltering<T>(this ListCollectionView collectionView, Predicate<T> filter) 
            where T:class            
        {
            collectionView.Filter = a=> filter(a as T);
            return collectionView;
        }

        public static ListCollectionView OrderBy(this ListCollectionView collectionView, IComparer comparer)
        {
            collectionView.CustomSort = comparer;
            return collectionView;
        }

    }
}