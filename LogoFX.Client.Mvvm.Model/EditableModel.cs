#if !WINDOWS_PHONE
#endif
using System;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    public partial class EditableModel<T> : Model<T>, IEditableModel
        where T : IEquatable<T>
    {        
        //private readonly Type Type;

        public EditableModel()
        {
            //Type = GetType();            
            InitDirtyListener();
        }                
    }

    public partial class EditableModel : EditableModel<int>
    {
        
    }
}