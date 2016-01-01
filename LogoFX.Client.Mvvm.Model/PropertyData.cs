using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    class PropertyData<T> : IPropertyData where T:IEquatable<T>
    {
        private string _description;
        private readonly object _object;
        private readonly PropertyInfo _propInfo;
        private readonly string _displayName;

        public PropertyData(Model<T> o, PropertyInfo prop)
        {
            _object = o;
            _propInfo = prop;

            DisplayAttribute a = prop.GetCustomAttributes(false).OfType<DisplayAttribute>().FirstOrDefault();
            if (a != null)
            {
                _description = a.Description;
                _displayName = a.Name;

            }
        }

        public object Value
        {
            get { return _propInfo.GetValue(_object, null); }
            set
            {
                _propInfo.SetValue(_object, value, null);

            }
        }

        public string Name
        {
            get { return _propInfo.Name; }
        }

        public bool HaveDescription
        {
            get { return !string.IsNullOrWhiteSpace(_description); }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string DisplayName
        {
            get { return _displayName ?? Name; }
        }
    }
}
