using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Caliburn.Micro;

namespace LogoFX.Client.Bootstrapping
{
    partial class BootstrapperContainerBase<TRootViewModel, TIocContainer>
    {
        private static readonly Dictionary<string, Type> _typedic = new Dictionary<string, Type>();

        private void DefineViewLocatorFunctions()
        {
            //overriden for performance reasons (Assembly caching)
            ViewLocator.LocateTypeForModelType = (modelType, displayLocation, context) =>
            {
                var viewTypeName = modelType.FullName.Substring(0, modelType.FullName.IndexOf("`") < 0
                    ? modelType.FullName.Length
                    : modelType.FullName.IndexOf("`")
                    ).Replace("Model", string.Empty);

                if (context != null)
                {
                    viewTypeName = viewTypeName.Remove(viewTypeName.Length - 4, 4);
                    viewTypeName = viewTypeName + "." + context;
                }

                Type viewType;
                if (!_typedic.TryGetValue(viewTypeName, out viewType))
                {
                    _typedic[viewTypeName] = viewType = (from assembly in AssemblySource.Instance
                                                         from type in assembly.GetExportedTypes()
                                                         where type.FullName == viewTypeName
                                                         select type).FirstOrDefault();
                }

                return viewType;
            };
            ViewLocator.LocateForModelType = (modelType, displayLocation, context) =>
            {
                var viewType = ViewLocator.LocateTypeForModelType(modelType, displayLocation, context);

                return viewType == null
                    ? new TextBlock
                    {
                        Text = string.Format("Cannot find view for\nModel: {0}\nContext: {1} .", modelType, context)
                    }
                    : ViewLocator.GetOrCreateViewType(viewType);
            };
        }
    }
}
