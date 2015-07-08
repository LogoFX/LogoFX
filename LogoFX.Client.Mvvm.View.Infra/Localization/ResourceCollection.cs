using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogoFX.Client.Mvvm.View.Infra.Localization
{
    /// <summary>
    /// Resources dictionary
    /// </summary>
    [Serializable]
    public sealed class ResourceCollection : Dictionary<string, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceCollection"/> class.
        /// </summary>
        public ResourceCollection()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceCollection"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        public ResourceCollection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
    }
}
