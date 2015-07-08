using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogoFX.Client.Mvvm.View.Infra.Localization
{
    /// <summary>
    /// Resources set collection
    /// </summary>
    [Serializable]
    public sealed class ResourceSetCollection : Dictionary<string, ResourceCollection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceSetCollection"/> class.
        /// </summary>
        public ResourceSetCollection()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceSetCollection"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        public ResourceSetCollection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
    }
}
