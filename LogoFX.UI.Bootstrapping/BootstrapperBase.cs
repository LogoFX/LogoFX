using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;

namespace LogoFX.UI.Bootstrapping
{
    /// <summary>
    /// Base class for bootstrapper that contains some global optimizations and assumptions
    /// </summary>
    /// <typeparam name="TRootViewModel"></typeparam>
    public class BootstrapperBase<TRootViewModel> :
#if !WinRT
 BootstrapperBase
#else
        CaliburnApplication
#endif
    {
        private readonly Dictionary<string, Type> _typedic = new Dictionary<string, Type>();

        protected BootstrapperBase()
        {
            Initialize();
        }        

        private void DisplayRootView()
        {
            DisplayRootViewFor(typeof(TRootViewModel));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            DisplayRootView();
        }

        /// <summary>
        /// Override to configure the framework and setup your <c>IoC</c> container.
        /// </summary>
        protected override void Configure()
        {
            base.Configure();
            //overrided for performance reasons (Assembly caching)
            ViewLocator.LocateTypeForModelType = (modelType, displayLocation, context) =>
            {
                Debug.Assert(modelType != null && modelType.FullName != null);
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
                    _typedic[viewTypeName] = viewType = (from assmebly in AssemblySource.Instance
                                                         from type in assmebly.GetExportedTypes()
                                                         where type.FullName == viewTypeName
                                                         select type).FirstOrDefault();
                }

                return viewType;
            };
            ViewLocator.LocateForModelType = (modelType, displayLocation, context) =>
            {
                var viewType = ViewLocator.LocateTypeForModelType(modelType, displayLocation, context);

                return viewType == null
                    ? new TextBlock { Text = string.Format("Cannot find view for\nModel: {0}\nContext: {1} .", modelType, context) }
                    : ViewLocator.GetOrCreateViewType(viewType);
            };
        }
    }
}