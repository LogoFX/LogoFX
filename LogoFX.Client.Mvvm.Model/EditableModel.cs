#if !WINDOWS_PHONE
#endif
using System;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    public partial class EditableModel<T> : Model<T>, IEditableModel
        where T : IEquatable<T>
    {                
        public EditableModel()
        {            
            InitDirtyListener();
        }                
    }

    public partial class EditableModel : EditableModel<int>
    {
        
    }
}