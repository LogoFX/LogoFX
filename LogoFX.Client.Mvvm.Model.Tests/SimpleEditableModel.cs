namespace LogoFX.Client.Mvvm.Model.Tests
{
    class SimpleEditableModel : EditableModel
    {
        public SimpleEditableModel(string name, int age)
            : this()
        {
            Name = name;
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
                _name = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => Error);
            }
        }
        public int Age { get; set; }
    }
}