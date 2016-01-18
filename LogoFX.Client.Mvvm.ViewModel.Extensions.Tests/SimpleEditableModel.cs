using System.Collections.Generic;
using LogoFX.Client.Mvvm.Model;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Tests
{
    interface ISimpleEditableModel : IEditableModel
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

    interface ICompositeEditableModel : IEditableModel
    {
        [EditableList]
        IEnumerable<int> Phones { get; }

        ISimpleEditableModel Person { get; set; }

        IEnumerable<ISimpleEditableModel> SimpleCollection { get; }
    }
}