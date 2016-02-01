using System;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Determines whether a collection should be stored during snapshot creation
    /// </summary>
    public class EditableListAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditableListAttribute"/> class.
        /// </summary>
        public EditableListAttribute()
        {
            CloneItems = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditableListAttribute"/> class.
        /// </summary>
        /// <param name="cloneItems">if set to <c>true</c> list items are cloned as well.</param>
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
