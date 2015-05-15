using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using LogoFX.Practices.IoC;

namespace LogoFX.Web.Core
{
    public sealed class IocHttpControllerActivator : IHttpControllerActivator
    {
        private readonly IServiceLocator _serviceLocator;

        #region Constructors

        public IocHttpControllerActivator(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        #endregion

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            IHttpController controller = _serviceLocator.GetInstance<IHttpController>(controllerType);

            request.RegisterForDispose(
                new Release(
                    () =>
                    {
                        IDisposable disposable = controller as IDisposable;
                        if (disposable != null)
                            disposable.Dispose();
                    }));

            return controller;
        }

        private sealed class Release : IDisposable
        {
            private readonly Action _release;

            public Release(Action release)
            {
                _release = release;
            }

            public void Dispose()
            {
                _release();
            }
        }
    }
}
