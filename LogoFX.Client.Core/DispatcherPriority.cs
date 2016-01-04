﻿ // ReSharper disable once CheckNamespace
namespace System.Windows.Threading
{
    /// <summary>
    /// Describes the priorities at which operations can be invoked by way of the <see cref="IDispatch"/>.
    /// </summary>
    public enum DispatcherPriority
    {
        /// <summary>
        /// The invalid
        /// </summary>
        Invalid = -1,
        /// <summary>
        /// The inactive
        /// </summary>
        Inactive = 0,
        /// <summary>
        /// The system idle
        /// </summary>
        SystemIdle = 1,
        /// <summary>
        /// The application idle
        /// </summary>
        ApplicationIdle = 2,
        /// <summary>
        /// The context idle
        /// </summary>
        ContextIdle = 3,
        /// <summary>
        /// The background
        /// </summary>
        Background = 4,
        /// <summary>
        /// The input
        /// </summary>
        Input = 5,
        /// <summary>
        /// The loaded
        /// </summary>
        Loaded = 6,
        /// <summary>
        /// The render
        /// </summary>
        Render = 7,
        /// <summary>
        /// The data bind
        /// </summary>
        DataBind = 8,
        /// <summary>
        /// The normal
        /// </summary>
        Normal = 9,
        /// <summary>
        /// The send
        /// </summary>
        Send = 10,
    }
}
