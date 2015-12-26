using System;

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// The exception that is thrown when the navigation service is not set
    /// </summary>
    public class NavigationServiceNullException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NavigationServiceNullException"/> class.
        /// </summary>
        public NavigationServiceNullException()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NavigationServiceNullException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public NavigationServiceNullException(string message)
            :base(message)
        {
            
        }
    }

    /// <summary>
    /// The exception that is thrown when trying to navigate to an unregistered type.
    /// </summary>
    public class UnregisteredTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="UnregisteredTypeException"/> class.
        /// </summary>
        public UnregisteredTypeException()
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="UnregisteredTypeException"/> class.
        /// </summary>
        /// <param name="message"></param>
        public UnregisteredTypeException(string message)
            :base(message)
        {

        }
    }
}