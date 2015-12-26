using System;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Selection mode
    /// </summary>
    [Flags]
    public enum SelectionMode : uint
    {
        /// <summary>
        /// Passive mode
        /// </summary>
        Passive = 0x0,
        /// <summary>
        /// Single mode
        /// </summary>
        One = 0x1,
        /// <summary>
        /// Single or no selection mode
        /// </summary>
        ZeroOrOne = 0x2,
        /// <summary>
        /// At least one item mode
        /// </summary>
        OneOrMore = 0x4,
        /// <summary>
        /// Any number of items mode
        /// </summary>
        ZeroOrMore = 0x8,

        /// <summary>
        /// Single selection
        /// </summary>
        [Obsolete("Use ZeroOrOne instead")]
        Single = ZeroOrOne,

        /// <summary>
        /// Multiple selection
        /// </summary>
        [Obsolete("Use ZeroOrMore instead")]
        Multiple = ZeroOrMore,

    }
}