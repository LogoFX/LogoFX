using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Solid.Practices.IoC;

namespace LogoFX.Server.Web.Bootstrapping
{
    class HttpControllerActivator : IHttpControllerActivator
    {
        private readonly IIocContainer _iocContainer;

        public HttpControllerActivator(IIocContainer iocContainer)
        {
            _iocContainer = iocContainer;            
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            IHttpController controller = _iocContainer.Resolve<IHttpController>();

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
