using TestStack.White;

namespace LogoFX.Client.Tests.EndToEnd.White
{
    //TODO: have to find better solution.     
    /// <summary>
    /// Ambient Context for <see cref="TestStack.White.Application"/>
    /// </summary>
    public static class ApplicationContext
    {
        /// <summary>
        /// Gets or sets the application.
        /// </summary>
        /// <value>
        /// The application.
        /// </value>
        public static Application Application { get; set; }
    }
}