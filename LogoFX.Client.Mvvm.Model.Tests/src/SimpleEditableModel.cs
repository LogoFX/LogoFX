using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model.Tests
{    
    interface ISimpleEditableModel : IEditableModel, ISimpleModel
    {        
        
    }        

    class SimpleEditableModel : EditableModel, ISimpleEditableModel
    {
        public SimpleEditableModel(string name, int age)
            : this()
        {
            _name = name;
            Age = age;
        }

        public SimpleEditableModel()
        {

        }

        private string _name;
        [NameValidation]
        public new string Name
        {
            get { return _name; }
            set
            {
                MakeDirty();
                _name = value;                
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => Error);
            }
        }
        public int Age { get; set; }
    }

    class SimpleEditableModelWithUndoRedo : EditableModel.WithUndoRedo, ISimpleEditableModel
    {        
        public SimpleEditableModelWithUndoRedo(string name, int age)
            : this()
        {
            _name = name;
            Age = age;
        }

        public SimpleEditableModelWithUndoRedo()
        {            
        }

        private string _name;
        [NameValidation]
        public new string Name
        {
            get { return _name; }
            set
            {
                MakeDirty();
                _name = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => Error);
            }
        }
        public int Age { get; set; }        
    }
}