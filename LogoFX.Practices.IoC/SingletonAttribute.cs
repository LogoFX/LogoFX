using System;

namespace LogoFX.Practices.IoC
{
    /// <summary>
    /// Use to mark the types to be registered as singleton
    /// during autoregistration using <see cref="ExtendedSimpleContainer"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SingletonAttribute : Attribute
    {
    }
}
