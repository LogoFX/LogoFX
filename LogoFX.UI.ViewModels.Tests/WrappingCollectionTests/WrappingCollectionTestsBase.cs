using System.Windows.Threading;
using LogoFX.UI.Tests.Infra;
using NUnit.Framework;

namespace LogoFX.UI.ViewModels.Tests.WrappingCollectionTests
{
    abstract class WrappingCollectionTestsBase
    {
        [SetUp]
        protected void Setup()
        {
            SetupCore();
            SetupOverride();
        }

        private void SetupCore()
        {
            Dispatch.Current = new SameThreadDispatch();
        }

        protected virtual void SetupOverride()
        {
            
        }
    }
}