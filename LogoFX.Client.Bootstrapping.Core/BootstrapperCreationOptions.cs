using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Represents various options fot bootstrapper creation.
    /// </summary>
    public class BootstrapperCreationOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperCreationOptions"/> class.
        /// </summary>
        public BootstrapperCreationOptions()
        {
            UseApplication = true;
            ReuseCompositionInformation = true;
            DiscoverCompositionModules = true;
            InspectAssemblies = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the real application is used. Use <c>false</c> for tests.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the real application is used; otherwise, <c>false</c>.
        /// </value>
        public bool UseApplication { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the composition information is re-used. Use <c>true</c>
        /// when running lots of integration client-side tests.
        /// </summary>
        /// <value>
        /// <c>true</c> if the composition information is re-used; otherwise, <c>false</c>.
        /// </value>
        public bool ReuseCompositionInformation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the bootstrapper 
        /// should look for instances of <see cref="ICompositionModule"/>.
        /// </summary>
        /// <value>
        /// <c>true</c> if the composition modules should be looked for; otherwise, <c>false</c>.
        /// </value>
        public bool DiscoverCompositionModules { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the bootstrapper
        /// should look for potential application-component assemblies.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the assemblies should be looked for; otherwise, <c>false</c>.
        /// </value>
        public bool InspectAssemblies { get; set; }
    }
}