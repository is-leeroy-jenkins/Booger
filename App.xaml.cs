// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 05-24-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        05-24-2024
// ******************************************************************************************
// <copyright file="App.xaml.cs" company="Terry D. Eppler">
//     Booger is a quick & dirty WPF application in C sharp for interacting with the OpenAI GPT API.
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
//    You can contact me at:   terryeppler@gmail.com or eppler.terry@epa.gov
// </copyright>
// <summary>
//   App.xaml.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System.Diagnostics.CodeAnalysis;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Windows;
    using RestoreWindowPlace;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    public partial class App : Application
    {
        /// <summary>
        /// The window place
        /// </summary>
        private WindowPlace? _windowPlace;

        /// <inheritdoc />
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" />
        /// that contains the event data.</param>
        protected override void OnStartup( StartupEventArgs e )
        {
            base.OnStartup( e );
            try
            {
                string _openaiApiKey;
                if( e.Args?.Length > 0
                    && e.Args[ 0 ].StartsWith( '/' ) )
                {
                    _openaiApiKey = e.Args[ 0 ].Remove( 0, 1 );
                }
                else
                {
                    _openaiApiKey = ConfigurationManager.AppSettings[ "AI" ] ?? string.Empty;
                }

                var _chatGptService = new GptService( _openaiApiKey );
                var _chatViewModel = new ChatViewModel( _chatGptService );
                var _mainWindow = new ChatWindow( _chatViewModel );
                SetupRestoreWindowPlace( _mainWindow );
                _mainWindow.Show( );
            }
            catch( Exception _ex )
            {
                MessageBox.Show( _ex.ToString( ), "Booger will exit on error" );
                Current?.Shutdown( );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Exit" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.Windows.ExitEventArgs" />
        /// that contains the event data.</param>
        protected override void OnExit( ExitEventArgs e )
        {
            base.OnExit( e );
            try
            {
                _windowPlace?.Save( );
            }
            catch( Exception _ex )
            {
                // skip
            }
        }

        /// <summary>
        /// Setups the restore window place.
        /// </summary>
        /// <param name="mainWindow">The main window.</param>
        private void SetupRestoreWindowPlace( ChatWindow mainWindow )
        {
            var _windowPlaceConfigFilePath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory,
                "Booger.config" );

            _windowPlace = new WindowPlace( _windowPlaceConfigFilePath );
            _windowPlace.Register( mainWindow );
            if( !File.Exists( _windowPlaceConfigFilePath ) )
            {
                mainWindow.WindowState = WindowState.Maximized;
            }
        }
    }
}