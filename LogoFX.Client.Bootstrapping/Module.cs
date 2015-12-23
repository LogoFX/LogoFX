using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using LogoFX.Core;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping
{
    [Export(typeof(ICompositionModule))]
    class CommonModule<TRootViewModel, TIocContainer> : ICompositionModule<TIocContainer> 
        where TRootViewModel : class 
        where TIocContainer : class, IIocContainer
    {
        public void RegisterModule(TIocContainer iocContainer)
        {
            iocContainer.RegisterSingleton<IWindowManager, WindowManager>();
            iocContainer.RegisterSingleton<TRootViewModel, TRootViewModel>();
            iocContainer.RegisterInstance<IIocContainer>(iocContainer);
            iocContainer.RegisterInstance(iocContainer);
        }
    }    

    [Export(typeof(ICompositionModule))]
    class ViewAndViewModelModule<TRootViewModel, TIocContainer> : ICompositionModule<TIocContainer> where TIocContainer : IIocContainer
    {
        public void RegisterModule(TIocContainer iocContainer)
        {
            AssemblySource.Instance
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type != typeof(TRootViewModel) && type.Name.EndsWith("ViewModel"))
                .Where(type => !(string.IsNullOrWhiteSpace(type.Namespace)) && type.Namespace != null && type.Namespace.EndsWith("ViewModels"))
                .Where(type => type.GetInterface(typeof(INotifyPropertyChanged).Name, false) != null)
                .ForEach(a => iocContainer.RegisterTransient(a, a));
        }
    }    
}
