using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using LogoFX.Client.Mvvm.Core;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    class CompositeEditableModel : EditableModel
    {
        private readonly Dictionary<string, PropertyInfo> _dataErrorInfoProps;

        public CompositeEditableModel(string location)
        {                          
            var type = GetType();
            var props = type.GetProperties();
            _dataErrorInfoProps =
                props.Where(t => t.PropertyType.GetInterfaces().Contains(typeof (IDataErrorInfo)))
                    .ToDictionary(t => t.Name, t => t);
            ListenToPropertyChange();
            Location = location; 
            Person = new SimpleEditableModel();           
        }

        private void ListenToPropertyChange()
        {
            PropertyChanged += WeakDelegate.From(OnPropertyChanged);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var changedPropertyName = e.PropertyName;
            if (_dataErrorInfoProps.ContainsKey(changedPropertyName) == false)
            {
                return;
            }
            var propertyValue = _dataErrorInfoProps[changedPropertyName].GetValue(this);
            if (propertyValue != null)
            {
                propertyValue.NotifyOn("Error", (o, o1) => NotifyOfPropertyChange(() => Error));
            }            
        }

        public override string Error
        {
            get
            {
                var ownError = base.Error;
                var childrenErrors = _dataErrorInfoProps.Select(t => (t.Value.GetValue(this) as IDataErrorInfo).Error);
                var stringBuilder = new StringBuilder();
                AppendErrorIfNeeded(ownError, stringBuilder);
                
                foreach (var childError in childrenErrors)
                {
                    AppendErrorIfNeeded(childError, stringBuilder);                  
                }
                return stringBuilder.ToString();
            }
        }

        private static void AppendErrorIfNeeded(string ownError, StringBuilder stringBuilder)
        {
            if (string.IsNullOrWhiteSpace(ownError) == false)
            {
                stringBuilder.AppendLine(ownError);
            }
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

    //use this attribute to mark the properties
    //that should contibute to the Error property
    //of the containing object
    [AttributeUsage(AttributeTargets.Property)]
    class IncludeErrorAttribute : Attribute
    {
        
    }
}