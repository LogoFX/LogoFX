using System;
using LogoFX.Client.Core;
using LogoFX.UI.Views.Infra.Localization;

namespace LogoFX.UI.Views.Infra.MarkupExtensions
{
    public sealed class LocalizedStringExtension : UpdatableMarkupExtension
    {
        #region Fields

        string _key = null;
        string _fallback = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedStringExtension"/> class.
        /// </summary>
        public LocalizedStringExtension() : this(string.Empty, string.Empty) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedStringExtension"/> class.
        /// </summary>
        /// <param name="key">The localized value will be found by this parameter.</param>
        public LocalizedStringExtension(string key) : this(key, string.Empty) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedStringExtension"/> class.
        /// </summary>
        /// <param name="key">The localized value will be found by this parameter.</param>
        /// <param name="fallback">The value that will be returned if the localization value not found.</param>
        public LocalizedStringExtension(string key, string fallback)
        {
            _key = key;
            _fallback = fallback;

            LocalizationManager.Instance.SubscribeToPropertyChanged(manager => manager.CurrentCulture, manager => UpdateValue(GetValue()));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Key property. 
        /// </summary>
        /// <value></value>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// Gets or sets a fallback string. 
        /// </summary>
        /// The <c>string</c> represents a key of the string resource. The fallback string is 
        /// used when a resource is not found.
        /// <value></value>
        public string Fallback
        {
            get { return _fallback; }
            set { _fallback = value; }
        }

        #endregion

        #region UpdatableMarkupExtension Implementation

        /// <summary>
        /// Provides the localization value by key specified in constructor.
        /// </summary>
        /// <param name="serviceProvider">This parameter is not used.</param>
        /// <returns>Localized value by key specified in constructor or fallback value. </returns>
        protected override object ProvideValueInternal(IServiceProvider serviceProvider)
        {
            return GetValue();
        }

        #endregion

        #region Private Members

        private object GetValue()
        {
            if (string.IsNullOrEmpty(_key))
            {
                return _fallback;
            }

            try
            {
                string resource = LocalizationManager.Instance.GetString(_key);

                if (String.IsNullOrEmpty(resource))
                {
                    return _fallback;
                }

                return resource;
            }

            catch (Exception)
            {
                return _fallback;
            }
        }

        #endregion
    }
}
