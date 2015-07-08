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
using System.Collections;
using LogoFX.Client.Mvvm.ViewModel.Interfaces;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Simple type safe <c>ViewModel</c> list
    /// </summary>
    /// <typeparam name="THead">head model type</typeparam>
    /// <typeparam name="TChild">children model type</typeparam>
    public class SimpleObjectsListViewModel<THead,TChild>:ObjectsListViewModel,IObjectViewModel<THead>
    {        
        private readonly Func<TChild, IObjectViewModel<TChild>> _modelCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleObjectsListViewModel&lt;THead, TChild&gt;"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="models">The models.</param>
        /// <param name="creator">The creator.</param>
        public SimpleObjectsListViewModel(THead obj, IEnumerable models, Func<TChild, IObjectViewModel<TChild>> creator = null)
            : base(obj, models)
        {            
            _modelCreator = creator;
        }

        /// <summary>
        /// Gets the object model.
        /// </summary>
        /// <value>The object model.</value>
        public  virtual THead ObjectModel
        {
            get { return (THead)base.InternalModel; }
        }

        /// <summary>
        /// Gets the object model.
        /// </summary>
        /// <value>The object model.</value>
        public new virtual THead Model
        {
            get { return (THead)base.InternalModel; }
        }

        /// <summary>
        /// Creates view model
        /// </summary>
        /// <param name="parent">Parent model</param>
        /// <param name="obj">Object for which we making model</param>
        /// <returns></returns>
        public override IObjectViewModel CreateViewModel(IViewModel parent, object obj)
        {
            return   _modelCreator!=null?_modelCreator((TChild)obj):new ObjectViewModel<TChild>((TChild)obj);            
        }
    }
}
