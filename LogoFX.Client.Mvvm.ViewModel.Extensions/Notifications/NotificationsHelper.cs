using System;
using System.Collections.Generic;
using LogoFX.Core;

// ReSharper disable once CheckNamespace
namespace LogoFX.Client.Mvvm.Notifications
{
    static class NotificationsHelper
    {
        private static readonly WeakKeyDictionary<object, Dictionary<string, Action<object, object>>> _notifiers =
            new WeakKeyDictionary<object, Dictionary<string, Action<object, object>>>();

        /// <summary>
        /// Subscribes supplied object to property changed notifications and invokes the provided callback        
        /// </summary>
        /// <typeparam name="T">Type of subject</typeparam><param name="subject">Subject</param><param name="path">Property path</param><param name="callback">Notification callback</param>
        internal static void NotifyOn<T>(this T subject, string path, Action<object, object> callback)
        {
            Dictionary<string, Action<object, object>> dictionary;
            if (!_notifiers.TryGetValue((object)subject, out dictionary))
                _notifiers.Add((object)subject, dictionary = new Dictionary<string, Action<object, object>>());
            dictionary.Remove(path);
            var notificationHelperDp = WeakDelegate.From(callback);            
            dictionary.Add(path, notificationHelperDp);
        }

        /// <summary>
        /// Unsubscribes supplied object from property changed notifications        
        /// </summary>
        /// <typeparam name="T">Type of subject</typeparam><param name="subject">Subject</param><param name="path">Property path</param>
        public static void UnNotifyOn<T>(this T subject, string path)
        {
            Dictionary<string, Action<object, object>> dictionary;
            if (!_notifiers.TryGetValue((object)subject, out dictionary) || !dictionary.ContainsKey(path))
                return;            
            dictionary.Remove(path);
        }
    }
}
