namespace LogoFX.Client.Mvvm.Model.Tests
{
    class CompositeEditableModel : EditableModel
    {       
        public CompositeEditableModel(string location)
        {                                      
            Location = location; 
            Person = new SimpleEditableModel();           
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
    }    
}