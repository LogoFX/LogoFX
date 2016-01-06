using System;
using System.Collections.Generic;
using System.Linq;

namespace LogoFX.Client.Mvvm.Model
{
    interface IErrorInfoExtractionStrategy
    {
        IEnumerable<string> ExtractChildrenErrors(Type type, object propertyContainer);
        bool IsPropertyErrorInfoSource(Type type, string propertyName);        
        object GetErrorInfoSourceValue<T>(Type type, string changedPropertyName, Model<T> model) where T : IEquatable<T>;
    }

#if NET45

    class DataErrorInfoExtractionStrategy : IErrorInfoExtractionStrategy
    {
        public IEnumerable<string> ExtractChildrenErrors(Type type, object propertyContainer)
        {
            return
                TypeInformationProvider.GetDataErrorInfoSourceValuesUnboxed(type, propertyContainer)
                    .Where(t => t != null)
                    .Select(t => t.Error)
                    .ToArray();
        }
    }

#endif

    class NotifyDataErrorInfoExtractionStrategy : IErrorInfoExtractionStrategy
    {
        public IEnumerable<string> ExtractChildrenErrors(Type type, object propertyContainer)
        {
            return
                TypeInformationProvider.GetNotifyDataErrorInfoSourceValuesUnboxed(type, propertyContainer)
                    .Where(t => t != null)
                    .Select(t => t.GetErrors(null))
                    .SelectMany(t => t.OfType<string>())
                    .ToArray();
        }

        public bool IsPropertyErrorInfoSource(Type type, string propertyName)
        {
            return TypeInformationProvider.IsPropertyNotifyDataErrorInfoSource(type, propertyName);
        }

        public object GetErrorInfoSourceValue<T>(Type type, string changedPropertyName, Model<T> model) where T : IEquatable<T>
        {
            return TypeInformationProvider.GetNotifyDataErrorInfoSourceValue(type, changedPropertyName, model);
        }
    }
}