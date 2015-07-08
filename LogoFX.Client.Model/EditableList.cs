using System;

namespace LogoFX.Client.Model
{
    public class EditableListAttribute:Attribute
    {
        private bool _cloneItems = false;

        public EditableListAttribute()
        {
            
        }

        public EditableListAttribute(bool cloneItemsToo)
        {
            _cloneItems = cloneItemsToo;
        }
        public bool CloneItems
        {
            set { _cloneItems = value; }
            get { return _cloneItems; }
        }
    }
}
