using System;
using System.Reflection;
using LogoFX.Client.Core;
using NUnit.Framework;

namespace LogoFX.UI.Core.Tests
{
    abstract class TestClassBase : NotifyPropertyChangedBase<TestClassBase>
    {        
        public abstract int Number { get;
            set;
        }

        public void Refresh()
        {
            NotifyOfPropertiesChange();
        }

        public void UpdateSilent(Action action)
        {
            using (SuppressNotify)
            {
                action();
            }
        }
    }

    class TestNameClass : TestClassBase
    {
        public override int Number
        {
            get { return 0; }
            set
            {
                NotifyOfPropertyChange("Number");
            }
        }
    }

    class TestPropertyInfoClass : TestClassBase
    {
        private readonly PropertyInfo _propertyInfo;

        public TestPropertyInfoClass()
        {
            _propertyInfo = GetType().GetProperty("Number");
        }

        public override int Number
        {
            get { return 0; }
            set
            {
                NotifyOfPropertyChange(_propertyInfo);
            }
        }
    }

    class TestExpressionClass : TestClassBase
    {
        public override int Number
        {
            get { return 0; }
            set
            {
                NotifyOfPropertyChange(() => Number);
            }
        }
    }

    [TestFixture]
    class NotifyPropertyChangedTests
    {
        [Test]
        [TestCaseSource("NpcIsRaisedCases")]
        public void PropertyChanged_PropertyValueIsChanged_NotificationIsRaisedIsTrue(
            TestClassBase testClass, bool expectedIsCalled)
        {            
            bool isCalled = false;
            testClass.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Number")
                {
                    isCalled = true;
                }
            };
                
            testClass.Number = 5;

            Assert.AreEqual(expectedIsCalled, isCalled);
        }

        [Test]
        [TestCaseSource("NpcIsNotRaisedCases")]
        public void PropertyChanged_PropertyValueIsChanged_NotificationIsRaised(
            TestClassBase testClass, bool expectedIsCalled)
        {
            bool isCalled = false;
            testClass.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Number")
                {
                    isCalled = true;
                }
            };

            testClass.UpdateSilent(() =>
            {
                testClass.Number = 5;    
            });
            
            Assert.AreEqual(expectedIsCalled, isCalled);
        }

        [Test]
        public void PropertyChanged_NotifyOfPropertiesChangeIsInvoked_EmptyNotificationIsRaised()
        {
            var testClass = new TestNameClass();
            bool isCalled = false;
            testClass.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "")
                {
                    isCalled = true;
                }
            };

            testClass.Refresh();

            Assert.IsTrue(isCalled);
        }

        private static readonly object[] NpcIsRaisedCases =
        {
            new object[] {new TestNameClass(), true},
            new object[] {new TestPropertyInfoClass(), true},            
            new object[] {new TestExpressionClass(), true}
        };

        private static readonly object[] NpcIsNotRaisedCases =
        {
            new object[] {new TestNameClass(), false},
            new object[] {new TestPropertyInfoClass(), false},            
            new object[] {new TestExpressionClass(), false}
        };
    }
}
