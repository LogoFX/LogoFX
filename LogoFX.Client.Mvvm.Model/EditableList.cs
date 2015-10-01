using System;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// Determines whether a collection should be stored during snapshot creation
    /// </summary>
    public class EditableListAttribute : Attribute
    {
        public EditableListAttribute()
        {
            CloneItems = false;
        }

        public EditableListAttribute(bool cloneItems)
        {
            CloneItems = cloneItems;
        }

        /// <summary>
        /// True if child items should be cloned during snapshot creation, false otherwise
        /// </summary>
        public bool CloneItems { set; get; }
    }
}
