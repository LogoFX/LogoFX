// ===================================
// <copyright>LogoUI Co.</copyright>
// <author>Vlad Spivak</author>
// <email>mailto:vlads@logoui.co.il</email>
// <created>21/00/10</created>
// <lastedit>Sunday, November 21, 2010</lastedit>

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the 'Software'), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//
// <remarks>Part of this software based on various internet sources, mostly on the works
// of members of Wpf Disciples group http://wpfdisciples.wordpress.com/
// Also project may contain code from the frameworks: 
//        Nito 
//        OpenLightGroup
//        nRoute
// </remarks>
// ====================================================================================//

using System.ComponentModel;

namespace LogoFX.UI.ViewModels.Interfaces
{
    /// <summary>
    /// Describes <c>ViewModel</c>
    /// </summary>
    public interface IViewModel:INotifyPropertyChanged,ISelectable
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>The display name.</value>
        string DisplayName { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expanded.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is expanded; otherwise, <c>false</c>.
        /// </value>
        bool IsExpanded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is current.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is current; otherwise, <c>false</c>.
        /// </value>
        bool IsCurrent { get; set; }

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is selected.
        ///// </summary>
        ///// <value>
        ///// 	<c>true</c> if this instance is selected; otherwise, <c>false</c>.
        ///// </value>
        //bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets the last error.
        /// </summary>
        /// <value>The last error.</value>
        string LastError { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is busy.
        /// </summary>
        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        bool IsBusy { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        IViewModel Parent { get; set; }
    }
}