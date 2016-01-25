using TestStack.White;

namespace LogoFX.Client.Tests.EndToEnd.White
{      
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