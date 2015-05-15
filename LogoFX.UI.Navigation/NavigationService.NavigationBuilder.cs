using System;
using System.Diagnostics;

namespace LogoFX.UI.Navigation
{
    internal sealed partial class NavigationService
    {
        private abstract class NavigationBuilder : INavigationBuilder
        {
            private bool _isRoot;
            private bool _isSingleton;
            private Type _conductorType;
            private object _value;

            public bool IsRoot
            {
                get { return _isRoot; }
                protected set
                {
                    Debug.Assert(!_isRoot, "You can set IsRoot property only once.");

                    _isRoot = value;
                }
            }

            public bool IsSingleton
            {
                get { return _isSingleton; }
                protected set
                {
                    Debug.Assert(!_isSingleton, "You can set IsSingleton property only once.");

                    _isSingleton = value;
                }
            }

            public Type ConductorType
            {
                get { return _conductorType; }
                protected set
                {
                    Debug.Assert(_conductorType == null, "You can set ConductorType property only once.");

                    _conductorType = value;
                }
            }

            public bool NotRemember { get; protected set; }

            object INavigationBuilder.GetValue()
            {
                if (IsSingleton)
                {
                    return _value ?? (_value = GetValue());
                }

                return GetValue();
            }

            protected abstract object GetValue();

            internal void DestroySingleton()
            {
                _value = null;
            }
        }
    }
}