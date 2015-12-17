using System;

namespace LogoFX.Client.Mvvm.Navigation
{
    public class NavigationServiceNullException : Exception
    {
        public NavigationServiceNullException()
        {
            
        }

        public NavigationServiceNullException(string message)
            :base(message)
        {
            
        }
    }

    public class UnregisteredTypeException : Exception
    {
        public UnregisteredTypeException()
        {

        }

        public UnregisteredTypeException(string message)
            :base(message)
        {

        }
    }
}