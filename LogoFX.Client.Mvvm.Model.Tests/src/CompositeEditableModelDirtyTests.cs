using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    [TestFixture]
    class CompositeEditableModelDirtyTests
    {
        [Test]
        public void InnerModelIsNotMadeDirty_IsDirtyIsFalse()
        {
            var compositeModel = new CompositeEditableModel("location");            

            Assert.IsFalse(compositeModel.IsDirty);
        }

        [Test]
        public void InnerModelIsMadeDirty_IsDirtyIsTrue()
        {
            var compositeModel = new CompositeEditableModel("location");
            compositeModel.Person.Name = DataGenerator.InvalidName;

            Assert.IsTrue(compositeModel.IsDirty);
        }

        [Test]
        public void InnerModelIsReset_DirtyNotificationIsRaised()
        {
            var compositeModel = new CompositeEditableModel("location");
            var isRaised = false;
            compositeModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "IsDirty")
                {
                    isRaised = true;
                }
            };
            compositeModel.Person = new SimpleEditableModel(DataGenerator.ValidName, 0);

            Assert.IsTrue(isRaised);
        }

        [Test]
        public void InnerModelIsMadeDirtyThenClearDirtyIsCalledWithChildrenEnforcement_IsDirtyIsFalse()
        {
            var compositeModel = new CompositeEditableModel("location");
            compositeModel.Person.Name = DataGenerator.InvalidName;
            compositeModel.ClearDirty(forceClearChildren:true);

            Assert.IsFalse(compositeModel.IsDirty);
        }

        [Test]
        public void InnerModelIsMadeDirtyThenClearDirtyIsCalledWithoutChildrenEnforcement_IsDirtyIsTrue()
        {
            var compositeModel = new CompositeEditableModel("location");
            compositeModel.Person.Name = DataGenerator.InvalidName;
            compositeModel.ClearDirty(forceClearChildren: false);

            Assert.IsTrue(compositeModel.IsDirty);
        }

        [Test]
        public void InnerModelInsideCollectionIsMadeDirty_IsDirtyIsTrue()
        {
            var simpleEditableModel = new SimpleEditableModel();
            var compositeModel = new CompositeEditableModel("location", new[] {simpleEditableModel});
            simpleEditableModel.Name = DataGenerator.InvalidName;            

            Assert.IsTrue(compositeModel.IsDirty);
        }        

        [Test]
        public void InnerModelInsideCollectionIsRemovedAndMadeDirty_IsDirtyIsTrue()
        {
            var simpleEditableModel = new SimpleEditableModel();
            var compositeModel = new CompositeEditableModel("location", new[] { simpleEditableModel });
            compositeModel.RemoveSimpleItem(simpleEditableModel);
            simpleEditableModel.Name = DataGenerator.InvalidName;

            Assert.IsTrue(compositeModel.IsDirty);
        }

        [Test]
        public void GivenInnerModelIsExplicitlyObservable_InnerModelInsideCollectionIsRemovedAndMadeDirty_IsDirtyIsTrue()
        {
            var simpleEditableModel = new SimpleEditableModel();
            var compositeModel = new ExplicitCompositeEditableModel("location", new[] { simpleEditableModel });
            compositeModel.RemoveSimpleItem(simpleEditableModel);
            simpleEditableModel.Name = DataGenerator.InvalidName;

            Assert.IsTrue(compositeModel.IsDirty);
        }

        [Test]
        public void InnerModelInsideCollectionIsRemovedAndModelDirtyIsClearedAndMadeDirty_IsDirtyIsFalse()
        {
            var simpleEditableModel = new SimpleEditableModel();
            var compositeModel = new CompositeEditableModel("location", new[] { simpleEditableModel });
            compositeModel.RemoveSimpleItem(simpleEditableModel);
            compositeModel.ClearDirty(true);
            simpleEditableModel.Name = DataGenerator.InvalidName;    
            
            Assert.IsFalse(compositeModel.IsDirty);
        }

        [Test]
        public void GivenInnerModelIsExplicitlyObservable_InnerModelInsideCollectionIsRemovedAndModelDirtyIsClearedAndMadeDirty_IsDirtyIsFalse()
        {
            var simpleEditableModel = new SimpleEditableModel();
            var compositeModel = new ExplicitCompositeEditableModel("location", new[] { simpleEditableModel });
            compositeModel.RemoveSimpleItem(simpleEditableModel);
            compositeModel.ClearDirty(true);
            simpleEditableModel.Name = DataGenerator.InvalidName;

            Assert.IsFalse(compositeModel.IsDirty);
        }

        [Test]
        public void GivenInnerModelIsExplicitlyObservable_InnerModelInsideCollectionIsRemoved_NotificationIsThrown()
        {
            var simpleEditableModel = new SimpleEditableModel();
            var compositeModel = new ExplicitCompositeEditableModel("location", new[] { simpleEditableModel });
            var flag = false;
            compositeModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "IsDirty")
                {
                    flag = true;
                }
            };
            compositeModel.RemoveSimpleItem(simpleEditableModel);            

            Assert.IsTrue(flag);
        }
    }
}