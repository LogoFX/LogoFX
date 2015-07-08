using System;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Selection mode
    /// </summary>
    [Flags]
    public enum SelectionMode : uint
    {
        Passive = 0x0,
        One = 0x1,
        ZeroOrOne = 0x2,
        OneOrMore = 0x4,
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