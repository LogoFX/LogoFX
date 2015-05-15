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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using LogoFX.Core;
using LogoFX.UI.ViewModels.Interfaces;

namespace LogoFX.UI.ViewModels
{   
    public class ModelProxyAttribute:Attribute
    {
        private string _property;

        public ModelProxyAttribute(string property)
        {
            _property = property;
        }

        public string Property
        {
            get { return _property; }
            set { _property = value; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectViewModel<T> : ObjectViewModel,IObjectViewModel<T>
    {
        #region Ctor's

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectViewModel&lt;T&gt;"/> class.
        /// </summary>
        public ObjectViewModel()
            : this(default(T))
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectViewModel&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ObjectViewModel(T model)
            : base(model)
        {
        }

        #endregion

        #region ObjectModel property

        /// <summary>
        /// ObjectModel property
        /// </summary>
#if !WinRT
        [Obsolete("Use Model instead")]
#endif
        public  T ObjectModel
        {
            get { return (T)base.InternalModel; }
        }

        /// <summary>
        /// Model property
        /// </summary>
#if WinRT
        [Obsolete("Use ObjectModel instead")]
#endif
        public new T Model
        {get
        {
#if WinRT
            throw new NotImplementedException();
#else
            return (T)InternalModel;
#endif
        }
        }



        #endregion
    }

    /// <summary>
    /// <c>ViewModel</c> that wraps arbitrary object
    /// </summary>
    [DebuggerDisplay("Model={_model}")]
    public class ObjectViewModel : ViewModelBase, IObjectViewModel
    {
        #region Ctor's

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectViewModel"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ObjectViewModel(object model)
        {
            _model = model;
            if(model is INotifyPropertyChanged)
            {
                (model as INotifyPropertyChanged).PropertyChanged += WeakDelegate.From(OnModelPropertyChangedCore);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectViewModel"/> class.
        /// </summary>
        public ObjectViewModel()
            : this(null)
        {
        }

        #endregion

        #region ObjectModel property

        private object _model;

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public  object Model
        {
            get { return InternalModel; }
            set
            {
                InternalModel = value;
            }
        }

        internal object InternalModel
        {
            get { return _model; }
            set
            {
                _model = value;
                NotifyOfPropertyChange(()=>DisplayName);
            }            
        }

        #endregion

        #region overrides

        /// <summary>
        /// DisplayName property
        /// </summary>
        /// <value></value>

        private string _externalDisplayName;
        [ModelProxy("Name")]
        public override string DisplayName
        {
            get
            {
                return _externalDisplayName ?? (InternalModel!=null?InternalModel.ToString():GetType().Name);
            }
            set
            {
                _externalDisplayName = value;
                NotifyOfPropertyChange(() => DisplayName);
            }
        }

        /// <summary>
        /// Called when some models property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        public event PropertyChangedEventHandler ModelsPropertyChanged;

        private void InvokeModelPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = ModelsPropertyChanged;
            if (handler != null) handler(this, e);
        }

        private Dictionary<string, IList<PropertyInfo>> _proxyNotifiers; 
#if SILVERLIGHT 
        public
#else
        private
#endif
        void OnModelPropertyChangedCore(object sender, PropertyChangedEventArgs e)
        {
            if(_proxyNotifiers == null)
            {
                Action<PropertyInfo> act = (pi) =>
                {
                    string pr = pi.GetCustomAttributes(typeof(ModelProxyAttribute), true).OfType<ModelProxyAttribute>().First().Property;
                    if (!_proxyNotifiers.ContainsKey(pr))
                        _proxyNotifiers.Add(pr, new List<PropertyInfo>());
                    _proxyNotifiers[pr].Add(pi);
                };

                _proxyNotifiers = new Dictionary<string, IList<PropertyInfo>>();
#if WinRT
                GetType().GetTypeInfo().DeclaredProperties
#else
                GetType().GetProperties(BindingFlags.Instance|BindingFlags.FlattenHierarchy|BindingFlags.Public)
#endif
                    .Where(a => a.IsDefined(typeof(ModelProxyAttribute), true))
                    .ForEach(act);
            }
            IList<PropertyInfo> toNotify;
            if(_proxyNotifiers.TryGetValue(e.PropertyName,out toNotify))
            {
                toNotify.ForEach(NotifyOfPropertyChange);
            }
            OnModelPropertyChanged(sender,e);
            InvokeModelPropertyChanged(e);
        }

        #endregion
    }
}
