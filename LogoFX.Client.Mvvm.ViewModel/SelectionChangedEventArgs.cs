// ===================================
// <copyright>LogoUI Co.</copyright>
// <author>Vlad Spivak</author>
// <email>mailto:vlads@logoui.co.il</email>
// <created>21/00/10</created>
// <lastedit>21/00/10</lastedit>

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

using System;
using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Selection notification event args
    /// </summary>
    /// <typeparam name="T">Type of selected items</typeparam>
    public class SelectionChangedEventArgs<T> : EventArgs
    {
        private readonly IEnumerable<T> _selection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionChangedEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="selection">The selection.</param>
        public SelectionChangedEventArgs(IEnumerable<T> selection)
        {
            _selection = selection;
        }

        /// <summary>
        /// Gets the selection.
        /// </summary>
        /// <value>The selection.</value>
        public IEnumerable<T> Selection
        {
            get { return _selection; }
        }
    }
}