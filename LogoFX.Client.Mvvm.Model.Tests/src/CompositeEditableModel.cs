using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    interface ICompositeEditableModel : IEditableModel
    {
        [EditableList]
        IEnumerable<int> Phones { get; }

        ISimpleEditableModel Person { get; set; }

        IEnumerable<ISimpleEditableModel> SimpleCollection { get; } 
    }

    class CompositeEditableModel : EditableModel, ICompositeEditableModel, ICloneable<CompositeEditableModel>, IEquatable<CompositeEditableModel>
    {       
        public CompositeEditableModel(string location)
        {                                      
            Location = location;
            _person = new SimpleEditableModel();            
        }

        public CompositeEditableModel(string location, IEnumerable<int> phones)
        {
            Location = location;
            _person = new SimpleEditableModel();            
            Phones.AddRange(phones);            
        }

        public CompositeEditableModel(string location, IEnumerable<SimpleEditableModel> simpleCollection)
        {
            Location = location;
            _person = new SimpleEditableModel();            
            foreach (var simpleEditableModel in simpleCollection)
            {
                _simpleCollection.Add(simpleEditableModel);
            }            
        }

        public string Location { get; private set; }

        private ISimpleEditableModel _person;        

        public ISimpleEditableModel Person
        {
            get { return _person; }
            set
            {
                MakeDirty();
                _person = value;
                NotifyOfPropertyChange();                
            }
        }

        private readonly ObservableCollection<SimpleEditableModel> _simpleCollection = new ObservableCollection<SimpleEditableModel>();

        private ObservableCollection<SimpleEditableModel> SimpleCollectionImpl
        {
            get { return _simpleCollection; }
        }

        public IEnumerable<ISimpleEditableModel> SimpleCollection
        {
            get { return SimpleCollectionImpl; }
        }

        private readonly List<int> _phones = new List<int>();
        
        IEnumerable<int> ICompositeEditableModel.Phones
        {
            get { return _phones; }
        }        
        private List<int> Phones
        {
            get { return _phones; }
        }

        public void AddPhone(int number)
        {
            MakeDirty();
            _phones.Add(number);
        }

        public void RemoveSimpleItem(SimpleEditableModel item)
        {
            MakeDirty();
            _simpleCollection.Remove(item);
        }

        public void AddSimpleModelImpl(SimpleEditableModel simpleEditableModel)
        {
            _simpleCollection.Add(simpleEditableModel);
        }

        public CompositeEditableModel Clone()
        {
            var composite = new CompositeEditableModel(Location, Phones);
            composite.Id = composite.Id;
            foreach (var simpleEditableModel in SimpleCollectionImpl)
            {
                composite.AddSimpleModelImpl(simpleEditableModel);
            }
            return composite;
        }

        public bool Equals(CompositeEditableModel other)
        {
            if (other == null)
            {
                return false;
            }
            return other.Id == Id;
        }
    }

    class ExplicitCompositeEditableModel : EditableModel, ICompositeEditableModel
    {
        public ExplicitCompositeEditableModel(string location)
        {
            Location = location;
            _person = new SimpleEditableModel();
        }

        public ExplicitCompositeEditableModel(string location, IEnumerable<int> phones)
        {
            Location = location;
            _person = new SimpleEditableModel();
            Phones.AddRange(phones);
        }

        public ExplicitCompositeEditableModel(string location, IEnumerable<SimpleEditableModel> simpleCollection)
        {
            Location = location;
            _person = new SimpleEditableModel();
            foreach (var simpleEditableModel in simpleCollection)
            {
                _simpleCollection.Add(simpleEditableModel);
            }
        }

        public string Location { get; private set; }

        private ISimpleEditableModel _person;

        public ISimpleEditableModel Person
        {
            get { return _person; }
            set
            {
                _person = value;
                NotifyOfPropertyChange();
            }
        }

        private readonly ObservableCollection<SimpleEditableModel> _simpleCollection = new ObservableCollection<SimpleEditableModel>();

        IEnumerable<ISimpleEditableModel> ICompositeEditableModel.SimpleCollection
        {
            get { return _simpleCollection; }
        }

        private readonly List<int> _phones = new List<int>();

        IEnumerable<int> ICompositeEditableModel.Phones
        {
            get { return _phones; }
        }
        private List<int> Phones
        {
            get { return _phones; }
        }

        public void AddPhone(int number)
        {
            MakeDirty();
            _phones.Add(number);
        }

        public void RemoveSimpleItem(SimpleEditableModel item)
        {
            MakeDirty();
            _simpleCollection.Remove(item);
        }
    }

    class CompositeEditableModelWithUndoRedo : EditableModel.WithUndoRedo, ICompositeEditableModel, ICloneable<CompositeEditableModelWithUndoRedo>, IEquatable<CompositeEditableModelWithUndoRedo>
    {
        public CompositeEditableModelWithUndoRedo(string location)
        {
            Location = location;
            _person = new SimpleEditableModel();
        }

        public CompositeEditableModelWithUndoRedo(string location, IEnumerable<int> phones)
        {
            Location = location;
            _person = new SimpleEditableModel();
            Phones.AddRange(phones);
        }

        public CompositeEditableModelWithUndoRedo(string location, IEnumerable<SimpleEditableModel> simpleCollection)
        {
            Location = location;
            _person = new SimpleEditableModel();
            foreach (var simpleEditableModel in simpleCollection)
            {
                _simpleCollection.Add(simpleEditableModel);
            }
        }

        public string Location { get; private set; }

        private ISimpleEditableModel _person;

        public ISimpleEditableModel Person
        {
            get { return _person; }
            set
            {
                MakeDirty();
                _person = value;
                NotifyOfPropertyChange();
            }
        }

        private readonly ObservableCollection<SimpleEditableModel> _simpleCollection = new ObservableCollection<SimpleEditableModel>();

        private ObservableCollection<SimpleEditableModel> SimpleCollectionImpl
        {
            get { return _simpleCollection; }
        }

        public IEnumerable<ISimpleEditableModel> SimpleCollection
        {
            get { return SimpleCollectionImpl; }
        }

        private readonly List<int> _phones = new List<int>();

        IEnumerable<int> ICompositeEditableModel.Phones
        {
            get { return _phones; }
        }
        private List<int> Phones
        {
            get { return _phones; }
        }

        public void AddPhone(int number)
        {
            MakeDirty();
            _phones.Add(number);
        }

        public void RemoveSimpleItem(SimpleEditableModel item)
        {
            MakeDirty();
            _simpleCollection.Remove(item);
        }

        public void AddSimpleModelImpl(SimpleEditableModel simpleEditableModel)
        {
            _simpleCollection.Add(simpleEditableModel);
        }

        public CompositeEditableModelWithUndoRedo Clone()
        {
            var composite = new CompositeEditableModelWithUndoRedo(Location, Phones);
            composite.Id = composite.Id;
            foreach (var simpleEditableModel in SimpleCollectionImpl)
            {
                composite.AddSimpleModelImpl(simpleEditableModel);
            }
            return composite;
        }

        public bool Equals(CompositeEditableModelWithUndoRedo other)
        {
            if (other == null)
            {
                return false;
            }
            return other.Id == Id;
        }
    }
}