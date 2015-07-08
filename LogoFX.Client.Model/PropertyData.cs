// ===================================
// <copyright>LogoUI Co.</copyright>
// <author>Vlad Spivak</author>
// <email>mailto:vlads@logoui.co.il</email>
// <created>22/00/10</created>
// <lastedit>22/00/10</lastedit>

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
//        Nito - http://nitokitchensink.codeplex.com/
//        OpenLightGroup - http://openlightgroup.net/
//        nRoute - http://nroute.codeplex.com/
//        SilverlightFX - http://projects.nikhilk.net/SilverlightFX
//        BlackLight - http://blacklight.codeplex.com/
// </remarks>
// ====================================================================================//

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using LogoFX.Client.Model.Contracts;

namespace LogoFX.Client.Model
{
    class PropertyData<T> : IPropertyData where T:IEquatable<T>
    {
        private string _description;
        private readonly object _object;
        private readonly PropertyInfo _propInfo;
        private readonly string _displayName;

        public PropertyData(Model<T> o, PropertyInfo prop)
        {
            _object = o;
            _propInfo = prop;

            DisplayAttribute a = prop.GetCustomAttributes(false).OfType<DisplayAttribute>().FirstOrDefault();
            if (a != null)
            {
                _description = a.Description;
                _displayName = a.Name;

            }
        }

        public object Value
        {
            get { return _propInfo.GetValue(_object, null); }
            set
            {
                _propInfo.SetValue(_object, value, null);

            }
        }

        public string Name
        {
            get { return _propInfo.Name; }
        }

        public bool HaveDescription
        {
            get { return !string.IsNullOrWhiteSpace(_description); }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string DisplayName
        {
            get { return _displayName ?? Name; }
        }
    }
}
