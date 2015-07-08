using LogoFX.Client.Core;
using NUnit.Framework;

namespace LogoFX.UI.Core.Tests
{
    [TestFixture]
    public class ReentranceGuardTests
    {
        [Test]
        public void AccessingSemaphore_SemaphoreIsRaisedOnce_IsLockedIsFalse()
        {
            var guard = new ReentranceGuard();
            guard.Raise();

            var isLocked = guard.IsLocked;
            Assert.IsFalse(isLocked);
        }

        [Test]
        public void AccessingSemaphore_SemaphoreIsRaisedTwice_IsLockedIsTrue()
        {
            var guard = new ReentranceGuard();
            guard.Raise();
            guard.Raise();

            var isLocked = guard.IsLocked;
            Assert.IsTrue(isLocked);
        }

        [Test]
        public void AccessingSemaphore_SemaphoreIsRaisedTrice_IsLockedIsTrue()
        {
            var guard = new ReentranceGuard();
            guard.Raise();
            guard.Raise();
            guard.Raise();

            var isLocked = guard.IsLocked;
            Assert.IsTrue(isLocked);
        }

        [Test]
        public void AccessingSemaphore_SemaphoreIsRaisedTwiceThenDisposeCalled_IsLockedIsFalse()
        {
            var guard = new ReentranceGuard();
            guard.Raise();
            using (guard.Raise())
            {
                
            }

            var isLocked = guard.IsLocked;
            Assert.IsFalse(isLocked);
        }
    }
}
