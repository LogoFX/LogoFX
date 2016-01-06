using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    interface ISimpleModel : IModel
    {
        new string Name { get; set; }
    }

    class SimpleModel : Model, ISimpleModel
    {
        public SimpleModel(string name, int age)
            : this()
        {
            _name = name;
            Age = age;
        }

        public SimpleModel()
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