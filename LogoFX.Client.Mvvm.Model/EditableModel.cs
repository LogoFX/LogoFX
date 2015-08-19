#if !WINDOWS_PHONE
#endif
using System;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    public partial class EditableModel<T> : Model<T>, IEditableModel
        where T : IEquatable<T>
    {        
        private readonly Type _type;

        public EditableModel()
        {
            _type = GetType();
            InitErrorListener();
            InitDirtyListener();
        }

        protected virtual void OnBeginEdit()
        {

        }

        protected virtual void OnEndEdit()
        {

        }

        protected virtual void OnCancelEdit()
        {

        }

        
    }

    public partial class EditableModel : EditableModel<int>
    {
        
    }
}