using System;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// Represents editable model.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class EditableModel<T> : Model<T>, IEditableModel
        where T : IEquatable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditableModel{T}"/> class.
        /// </summary>
        public EditableModel()
        {            
            InitDirtyListener();
            _history = new UndoRedoHistory<EditableModel<T>>(this);            
        }        
    }

    /// <summary>
    /// Represents editable model with <see cref="int"/> as identifier.
    /// </summary>
    public class EditableModel : EditableModel<int>
    {
        
    }
}