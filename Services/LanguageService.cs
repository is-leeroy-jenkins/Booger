// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="LanguageService.cs" company="Terry D. Eppler">
//     Booger is a quick & dirty WPF application that interacts with OpenAI GPT-3.5 Turbo API
//     based on NET6 and written in C-Sharp.
// 
//     Copyright ©  2022 Terry D. Eppler
// 
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the “Software”),
//    to deal in the Software without restriction,
//    including without limitation the rights to use,
//    copy, modify, merge, publish, distribute, sublicense,
//    and/or sell copies of the Software,
//    and to permit persons to whom the Software is furnished to do so,
//    subject to the following conditions:
// 
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
// 
//    THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//    INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT.
//    IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
//    DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//    DEALINGS IN THE SOFTWARE.
// 
//    You can contact me at:  terryeppler@gmail.com or eppler.terry@epa.gov
// </copyright>
// <summary>
//   LanguageService.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Local" ) ]
    public class LanguageService
    {
        /// <summary>
        /// The resource URI prefix
        /// </summary>
        private static string _resourceUriPrefix = "pack://application:,,,";

        /// <summary>
        /// The language resources
        /// </summary>
        private static Dictionary<CultureInfo, ResourceDictionary> _languageResources =
            new Dictionary<CultureInfo, ResourceDictionary>( )
            {
                {
                    new CultureInfo( "en" ), new ResourceDictionary( )
                    {
                        Source = new Uri( $"{_resourceUriPrefix}/Resources/Languages/en.xaml" )
                    }
                },
                {
                    new CultureInfo( "zh-hans" ), new ResourceDictionary( )
                    {
                        Source = new Uri( $"{_resourceUriPrefix}/Resources/Languages/zh-hans.xaml" )
                    }
                },
                {
                    new CultureInfo( "zh-hant" ), new ResourceDictionary( )
                    {
                        Source = new Uri( $"{_resourceUriPrefix}/Resources/Languages/zh-hant.xaml" )
                    }
                },
                {
                    new CultureInfo( "ja" ), new ResourceDictionary( )
                    {
                        Source = new Uri( $"{_resourceUriPrefix}/Resources/Languages/ja.xaml" )
                    }
                },
                {
                    new CultureInfo( "ar" ), new ResourceDictionary( )
                    {
                        Source = new Uri( $"{_resourceUriPrefix}/Resources/Languages/ar.xaml" )
                    }
                },
                {
                    new CultureInfo( "es" ), new ResourceDictionary( )
                    {
                        Source = new Uri( $"{_resourceUriPrefix}/Resources/Languages/es.xaml" )
                    }
                },
                {
                    new CultureInfo( "fr" ), new ResourceDictionary( )
                    {
                        Source = new Uri( $"{_resourceUriPrefix}/Resources/Languages/fr.xaml" )
                    }
                },
                {
                    new CultureInfo( "ru" ), new ResourceDictionary( )
                    {
                        Source = new Uri( $"{_resourceUriPrefix}/Resources/Languages/ru.xaml" )
                    }
                },
                {
                    new CultureInfo( "ur" ), new ResourceDictionary( )
                    {
                        Source = new Uri( $"{_resourceUriPrefix}/Resources/Languages/ur.xaml" )
                    }
                },
                {
                    new CultureInfo( "tr" ), new ResourceDictionary( )
                    {
                        Source = new Uri( $"{_resourceUriPrefix}/Resources/Languages/tr.xaml" )
                    }
                }
            };

        /// <summary>
        /// The default language
        /// </summary>
        private static CultureInfo _defaultLanguage = new CultureInfo( "en" );

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageService" /> class.
        /// </summary>
        /// <param name="configurationService">The configuration service.</param>
        public LanguageService( ConfigurationService configurationService )
        {
            ConfigurationService = configurationService;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Init( )
        {
            var _language = CultureInfo.CurrentCulture;
            if( !string.IsNullOrWhiteSpace( ConfigurationService.Configuration.Language ) )
            {
                _language = new CultureInfo( ConfigurationService.Configuration.Language );
            }

            SetLanguage( _language );
        }

        /// <summary>
        /// Gets the configuration service.
        /// </summary>
        /// <value>
        /// The configuration service.
        /// </value>
        private ConfigurationService ConfigurationService { get; }

        /// <summary>
        /// Gets the languages.
        /// </summary>
        /// <value>
        /// The languages.
        /// </value>
        public IEnumerable<CultureInfo> Languages
        {
            get
            {
                return _languageResources.Keys;
            }
        }

        /// <summary>
        /// The current language
        /// </summary>
        private CultureInfo _currentLanguage = _defaultLanguage;

        /// <summary>
        /// Gets or sets the current language.
        /// </summary>
        /// <value>
        /// The current language.
        /// </value>
        /// <exception cref="System.ArgumentException">Unsupport language</exception>
        public CultureInfo CurrentLanguage
        {
            get
            {
                return _currentLanguage;
            }
            set
            {
                if( !SetLanguage( value ) )
                {
                    throw new ArgumentException( "Unsupport language" );
                }
            }
        }

        /// <summary>
        /// Sets the language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public bool SetLanguage( CultureInfo language )
        {
            var _key = Languages.Where( key => key.Equals( language ) ).FirstOrDefault( );
            if( _key == null )
            {
                _key = Languages
                    .Where( key =>
                        key.TwoLetterISOLanguageName == language.TwoLetterISOLanguageName )
                    .FirstOrDefault( );
            }

            if( _key != null )
            {
                var _resourceDictionary = _languageResources[ _key ];
                var _oldLanguageResources = Application.Current.Resources.MergedDictionaries
                    .Where( dict => dict.Contains( "IsLanguageResource" ) ).ToList( );

                foreach( var _res in _oldLanguageResources )
                {
                    Application.Current.Resources.MergedDictionaries.Remove( _res );
                }

                Application.Current.Resources.MergedDictionaries.Add( _resourceDictionary );
                _currentLanguage = _key;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private protected void Fail( Exception ex )
        {
            var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}