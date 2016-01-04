#if NET45
using System;
#endif

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents filter model
    /// </summary>
    public interface IFilterModel : IValueObject
#if NET45
          , ICloneable
#endif      
    {

    }
}
