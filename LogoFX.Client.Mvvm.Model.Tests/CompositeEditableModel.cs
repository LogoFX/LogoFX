using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    interface ICompositeEditableModel
    {
        [EditableList]
        IEnumerable<int> Phones { get; } 
    }

    class CompositeEditableModel : EditableModel, ICompositeEditableModel
    {       
        public CompositeEditableModel(string location)
        {                                      
            Location = location; 
            Person = new SimpleEditableModel();           
        }

        public CompositeEditableModel(string location, IEnumerable<int> phones)
        {
            Location = location;
            Person = new SimpleEditableModel();
            Phones.AddRange(phones);
        }

        public string Location { get; private set; }

        private SimpleEditableModel _person;        

        public SimpleEditableModel Person
        {
            get { return _person; }
            set
            {
                _person = value;
                NotifyOfPropertyChange();
            }
        }

        private List<int> _phones;
        
        IEnumerable<int> ICompositeEditableModel.Phones
        {
            get { return _phones; }
        }

        private List<int> Phones
        {
            get { return _phones ?? (_phones = new List<int>()); }
        }

        public void AddPhone(int number)
        {
            MakeDirty();
            _phones.Add(number);
        }
    }    
}