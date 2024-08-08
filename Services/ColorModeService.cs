// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="ColorModeService.cs" company="Terry D. Eppler">
//    Booger is a quick & dirty WPF application that interacts with OpenAI GPT-3.5 Turbo API
//    based on NET6 and written in C-Sharp.
// 
//    Copyright ©  2024  Terry D. Eppler
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
//    You can contact me at: terryeppler@gmail.com or eppler.terry@epa.gov
// </copyright>
// <summary>
//   ColorModeService.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows;
    using System.Windows.Interop;
    using Microsoft.Win32;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class ColorModeService
    {
        /// <summary>
        /// The resource URI prefix
        /// </summary>
        private static string _resourceUriPrefix = "pack://application:,,,";

        /// <summary>
        /// The light mode
        /// </summary>
        private ResourceDictionary _light =
            new ResourceDictionary( )
            {
                Source = new Uri( $"{_resourceUriPrefix}/UI/ColorModes/LightMode.xaml" )
            };

        /// <summary>
        /// The dark mode
        /// </summary>
        private ResourceDictionary _dark =
            new ResourceDictionary( )
            {
                Source = new Uri( $"{_resourceUriPrefix}/UI/ColorModes/DarkMode.xaml" )
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorModeService"/> class.
        /// </summary>
        /// <param name="configurationService">The configuration service.</param>
        public ColorModeService( ConfigurationService configurationService )
        {
            ConfigurationService = configurationService;
        }

        /// <summary>
        /// Gets the configuration service.
        /// </summary>
        /// <value>
        /// The configuration service.
        /// </value>
        private ConfigurationService ConfigurationService { get; }

        /// <summary>
        /// Initializes the message hook.
        /// </summary>
        private void InitMessageHook( )
        {
            SystemEvents.UserPreferenceChanged +=
                SystemEvents_UserPreferenceChanged;
        }

        /// <summary>
        /// Handles the UserPreferenceChanged event of the SystemEvents control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="UserPreferenceChangedEventArgs"/> instance containing the event data.</param>
        private void SystemEvents_UserPreferenceChanged( object sender,
            UserPreferenceChangedEventArgs e )
        {
            if( e.Category == UserPreferenceCategory.General )
            {
                SwitchTo( CurrentMode );
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Init( )
        {
            var _configurationColorMode =
                ConfigurationService.Configuration.ColorMode;

            SwitchTo( _configurationColorMode );
            InitMessageHook( );
        }

        /// <summary>
        /// Gets the color modes.
        /// </summary>
        /// <value>
        /// The color modes.
        /// </value>
        public IEnumerable<ColorMode> ColorModes
        {
            get
            {
                return Enum.GetValues<ColorMode>( );
            }
        }

        /// <summary>
        /// The current mode
        /// </summary>
        private ColorMode _currentMode = ColorMode.Auto;

        /// <summary>
        /// The current actual mode
        /// </summary>
        private ColorMode _currentActualMode =
            SystemHelper.IsDarkTheme( )
                ? ColorMode.Dark
                : ColorMode.Light;

        /// <summary>
        /// Gets or sets the current mode.
        /// </summary>
        /// <value>
        /// The current mode.
        /// </value>
        public ColorMode CurrentMode
        {
            get
            {
                return _currentMode;
            }
            set
            {
                SwitchTo( value );
            }
        }

        /// <summary>
        /// Gets the current actual mode.
        /// </summary>
        /// <value>
        /// The current actual mode.
        /// </value>
        public ColorMode CurrentActualMode
        {
            get
            {
                return _currentActualMode;
            }
        }

        /// <summary>
        /// Switches to.
        /// </summary>
        /// <param name="colorModeResource">The color mode resource.</param>
        private void SwitchTo( ResourceDictionary colorModeResource )
        {
            var _oldColorModeResources =
                Application.Current.Resources.MergedDictionaries
                    .Where( dict => dict.Contains( "IsColorModeResource" ) )
                    .ToList( );

            foreach( var _res in _oldColorModeResources )
            {
                Application.Current.Resources.MergedDictionaries.Remove( _res );
            }

            Application.Current.Resources.MergedDictionaries.Add( colorModeResource );
        }

        /// <summary>
        /// Switches to.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <exception cref="System.ArgumentException">Must be bright or dark</exception>
        public void SwitchTo( ColorMode mode )
        {
            switch( mode )
            {
                case ColorMode.Light:
                    SwitchToLightMode( );
                    break;
                case ColorMode.Dark:
                    SwitchToDarkMode( );
                    break;
                case ColorMode.Auto:
                    SwitchToAuto( );
                    break;
                default:
                    throw new ArgumentException( "Must be bright or dark" );
            }
        }

        /// <summary>
        /// Applies the theme for window.
        /// </summary>
        /// <param name="window">The window.</param>
        public void ApplyThemeForWindow( Window window )
        {
            var _hwnd =
                new WindowInteropHelper( window ).Handle;

            if( _hwnd != IntPtr.Zero )
            {
                NativeMethods.EnableDarkModeForWindow( _hwnd, CurrentActualMode == ColorMode.Dark );
            }
            else
            {
                EventHandler _handler = null;
                _handler = ( s, args ) =>
                {
                    if( s is Window _window )
                    {
                        ApplyThemeForWindow( _window );
                    }

                    window.SourceInitialized -= _handler;
                };

                window.SourceInitialized += _handler;
            }
        }

        /// <summary>
        /// Switches to light mode core.
        /// </summary>
        /// <param name="setField">if set to <c>true</c> [set field].</param>
        private void SwitchToLightModeCore( bool setField )
        {
            SwitchTo( _light );
            if( setField )
            {
                ChangeColorModeAndNotify( ColorMode.Light, ColorMode.Light );
            }
        }

        /// <summary>
        /// Switches to dark mode core.
        /// </summary>
        /// <param name="setField">if set to <c>true</c> [set field].</param>
        private void SwitchToDarkModeCore( bool setField )
        {
            SwitchTo( _dark );
            if( setField )
            {
                ChangeColorModeAndNotify( ColorMode.Dark, ColorMode.Dark );
            }
        }

        /// <summary>
        /// Changes the color mode and notify.
        /// </summary>
        /// <param name="colorMode">The color mode.</param>
        /// <param name="actualColorMode">The actual color mode.</param>
        /// <exception cref="System.ArgumentException">actualColorMode</exception>
        private void ChangeColorModeAndNotify( ColorMode colorMode, ColorMode actualColorMode )
        {
            if( actualColorMode == ColorMode.Auto )
            {
                throw new ArgumentException( $"{nameof( actualColorMode )} cannot be 'Auto'",
                    nameof( actualColorMode ) );
            }

            var _notify = colorMode != _currentMode || actualColorMode != _currentActualMode;
            _currentMode = colorMode;
            _currentActualMode = actualColorMode;
            if( _notify )
            {
                ColorModeChanged?.Invoke( this,
                    new ColorModeChangedEventArgs( colorMode, actualColorMode ) );
            }
        }

        /// <summary>
        /// Switches to automatic.
        /// </summary>
        public void SwitchToAuto( )
        {
            var _isDarkMode = (bool)SystemHelper.IsDarkTheme( );
            if( _isDarkMode )
            {
                SwitchToDarkModeCore( false );
            }
            else
            {
                SwitchToLightModeCore( false );
            }

            ChangeColorModeAndNotify( ColorMode.Auto, _isDarkMode
                ? ColorMode.Dark
                : ColorMode.Light );

            foreach( Window _window in Application.Current.Windows )
            {
                ApplyThemeForWindow( _window );
            }
        }

        /// <summary>
        /// Switches to light mode.
        /// </summary>
        public void SwitchToLightMode( )
        {
            SwitchToLightModeCore( true );
            foreach( Window _window in Application.Current.Windows )
            {
                ApplyThemeForWindow( _window );
            }
        }

        /// <summary>
        /// Switches to dark mode.
        /// </summary>
        public void SwitchToDarkMode( )
        {
            SwitchToDarkModeCore( true );
            foreach( Window _window in Application.Current.Windows )
            {
                ApplyThemeForWindow( _window );
            }
        }

        /// <summary>
        /// Occurs when [color mode changed].
        /// </summary>
        public event EventHandler<ColorModeChangedEventArgs> ColorModeChanged;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ColorModeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorModeChangedEventArgs"/> class.
        /// </summary>
        /// <param name="colorMode">The color mode.</param>
        /// <param name="actualColorMode">The actual color mode.</param>
        public ColorModeChangedEventArgs( ColorMode colorMode, ColorMode actualColorMode )
        {
            ColorMode = colorMode;
            ActualColorMode = actualColorMode;
        }

        /// <summary>
        /// Gets the color mode.
        /// </summary>
        /// <value>
        /// The color mode.
        /// </value>
        public ColorMode ColorMode { get; }

        /// <summary>
        /// Gets the actual color mode.
        /// </summary>
        /// <value>
        /// The actual color mode.
        /// </value>
        public ColorMode ActualColorMode { get; }
    }
}