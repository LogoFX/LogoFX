using System;

namespace LogoFX.Practices.IoC
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SingletonAttribute : Attribute
    {
    }
}
