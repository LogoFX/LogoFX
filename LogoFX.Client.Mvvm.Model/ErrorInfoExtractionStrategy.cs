using System;
using System.Collections.Generic;
using System.Linq;

namespace LogoFX.Client.Mvvm.Model
{
    interface IErrorInfoExtractionStrategy
    {
        IEnumerable<string> ExtractChildrenErrors(Type type, object propertyContainer);
    }

    class DataErrorInfoExtractionStrategy : IErrorInfoExtractionStrategy
    {
        public IEnumerable<string> ExtractChildrenErrors(Type type, object propertyContainer)
        {
            return
                TypeInformationProvider.GetDataErrorInfoSourceValuesUnboxed(type, propertyContainer).Select(t => t.Error).ToArray();
        }
    }

    class NotifyDataErrorInfoExtractionStrategy : IErrorInfoExtractionStrategy
    {
        public IEnumerable<string> ExtractChildrenErrors(Type type, object propertyContainer)
        {
            return
                TypeInformationProvider.GetNotifyDataErrorInfoSourceValuesUnboxed(type, propertyContainer)
                    .Select(t => t.GetErrors(null))
                    .SelectMany(t => t.OfType<string>())
                    .ToArray();
        }
    }
}