﻿// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="ErrorWindow.xaml.cs" company="Terry D. Eppler">
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
//   ErrorWindow.xaml.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    [ SuppressMessage( "ReSharper", "InconsistentNaming" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "UnusedParameter.Global" ) ]
    public partial class ErrorWindow : Window, IDisposable
    {
        /// <summary>
        /// The busy
        /// </summary>
        private bool _busy;

        /// <summary>
        /// The locked object
        /// </summary>
        private object _path;

        /// <summary>
        /// The back color
        /// </summary>
        private protected Color _backColor = new Color( )
        {
            A = 255,
            R = 43,
            G = 0,
            B = 0
        };

        /// <summary>
        /// The back hover color
        /// </summary>
        private protected Color _backHover = new Color( )
        {
            A = 255,
            R = 128,
            G = 0,
            B = 0
        };

        /// <summary>
        /// The border color
        /// </summary>
        private protected Color _borderColor = new Color( )
        {
            A = 255,
            R = 255,
            G = 0,
            B = 0
        };

        /// <summary>
        /// The border hover color
        /// </summary>
        private protected Color _borderHover = new Color( )
        {
            A = 255,
            R = 128,
            G = 0,
            B = 0
        };

        /// <summary>
        /// The exception
        /// </summary>
        private protected Exception _exception;

        /// <summary>
        /// The fore color
        /// </summary>
        private protected Color _foreColor = new Color( )
        {
            A = 255,
            R = 255,
            G = 255,
            B = 255
        };

        /// <summary>
        /// The fore hover color
        /// </summary>
        private protected Color _foreHover = new Color( )
        {
            A = 255,
            R = 255,
            G = 255,
            B = 255
        };

        /// <summary>
        /// The message
        /// </summary>
        private protected string _message;

        /// <summary>
        /// The status update
        /// </summary>
        private protected Action _statusUpdate;

        /// <summary>
        /// The title
        /// </summary>
        private protected string _text;

        /// <summary>
        /// The theme
        /// </summary>
        private protected readonly DarkMode _theme = new DarkMode( );

        /// <summary>
        /// The timer
        /// </summary>
        private protected Timer _timer;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Booger.ErrorWindow" /> class.
        /// </summary>
        public ErrorWindow( )
        {
            InitializeComponent( );
            InitializeDelegates( );
            RegisterCallbacks( );

            // Basic Properties
            Width = 560;
            Height = 250;
            FontFamily = new FontFamily( "Roboto" );
            FontSize = 12d;
            WindowStyle = _theme.WindowStyle;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            Background = _theme.DarkRedBrush;
            Foreground = _theme.Foreground;
            BorderBrush = _theme.RedBrush;
            Topmost = true;
            ToolTip = "click to clear";

            // Event Wiring
            IsVisibleChanged += OnVisibleChanged;
            MouseLeftButtonDown += OnClick;
            MouseRightButtonDown += OnClick;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Booger.ErrorWindow" />
        /// class.
        /// </summary>
        /// <param name="exception"> The exception. </param>
        public ErrorWindow( Exception exception )
            : this( )
        {
            _exception = exception;
            MessageText.Content = exception.ToLogString( exception.Message );
            Header.Content = "There has been an error!";
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Booger.ErrorWindow" /> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="title">The title.</param>
        public ErrorWindow( Exception exception, string title )
            : this( exception )
        {
            _exception = exception;
            MessageText.Content = exception.ToLogString( exception.Message );
            Header.Content = title;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Booger.ErrorWindow" />
        /// class.
        /// </summary>
        /// <param name="message"> The message. </param>
        public ErrorWindow( string message )
            : this( )
        {
            MessageText.Content = message;
            Header.Content = "There has been an error!";
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Booger.ErrorWindow" /> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        public ErrorWindow( string title, string message )
            : this( )
        {
            Header.Content = title;
            MessageText.Content = message;
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        public void SetText( )
        {
            try
            {
                MessageText.Content = _exception?.ToLogString( "" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary> Called when [load]. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e">
        /// The
        /// <see cref="EventArgs"/>
        /// instance containing the event data.
        /// </param>
        public void OnVisibleChanged( object sender, DependencyPropertyChangedEventArgs e )
        {
            try
            {
                Opacity = 0;
                InitializeTimer( );
                FadeInAsync( this );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Initializes the timer.
        /// </summary>
        private void InitializeTimer( )
        {
            try
            {
                var _callback = new TimerCallback( UpdateStatus );
                _timer = new Timer( _callback, null, 0, 80 );
            }
            catch( Exception ex )
            {
                _timer?.Dispose( );
                Fail( ex );
            }
        }

        /// <summary>
        /// Initializes the callbacks.
        /// </summary>
        private void RegisterCallbacks( )
        {
            try
            {
                //CloseButton.Click += OnCloseButtonClick;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Initializes the delegates.
        /// </summary>
        private void InitializeDelegates( )
        {
            try
            {
                _statusUpdate += UpdateStatus;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Initializes the labels.
        /// </summary>
        private void InitializeLabels( )
        {
            try
            {
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Initializes the text box.
        /// </summary>
        private void InitializeTextBoxes( )
        {
            try
            {
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Fades the in asynchronous.
        /// </summary>
        /// <param name="form">The o.</param>
        /// <param name="interval">The interval.</param>
        private async void FadeInAsync( Window form, int interval = 80 )
        {
            try
            {
                ThrowIf.Null( form, nameof( form ) );
                while( form.Opacity < 1.0 )
                {
                    await Task.Delay( interval );
                    form.Opacity += 0.05;
                }

                form.Opacity = 1;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Fades the out asynchronous.
        /// </summary>
        /// <param name="window">The o.</param>
        /// <param name="interval">The interval.</param>
        private async void FadeOutAsync( Window window, int interval = 80 )
        {
            try
            {
                ThrowIf.Null( window, nameof( window ) );
                while( window.Opacity > 0.0 )
                {
                    await Task.Delay( interval );
                    window.Opacity -= 0.05;
                }

                window.Opacity = 0;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Begins the initialize.
        /// </summary>
        private void Busy( )
        {
            try
            {
                if( _path == null )
                {
                    _path = new object( );
                    lock( _path )
                    {
                        _busy = true;
                    }
                }
                else
                {
                    lock( _path )
                    {
                        _busy = true;
                    }
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        private void UpdateStatus( )
        {
            try
            {
                StatusLabel.Content = DateTime.Now.ToLongTimeString( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        private void UpdateStatus( object state )
        {
            try
            {
                Dispatcher.BeginInvoke( _statusUpdate );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/>
        /// instance containing the event data.</param>
        private void OnClick( object sender, MouseEventArgs e )
        {
            try
            {
                Close( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex"> The ex. </param>
        private void Fail( Exception ex )
        {
            Console.WriteLine( ex.Message );
        }

        /// <summary>
        /// Invokes if needed.
        /// </summary>
        /// <param name="action">The action.</param>
        private protected void InvokeIf( Action action )
        {
            try
            {
                ThrowIf.Null( action, nameof( action ) );
                if( Dispatcher.CheckAccess( ) )
                {
                    action?.Invoke( );
                }
                else
                {
                    Dispatcher.BeginInvoke( action );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Ends the initialize.
        /// </summary>
        private protected void Chill( )
        {
            try
            {
                if( _path == null )
                {
                    _path = new object( );
                    lock( _path )
                    {
                        _busy = false;
                    }
                }
                else
                {
                    lock( _path )
                    {
                        _busy = false;
                    }
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Performs application-defined tasks
        /// associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c>
        /// to release both managed
        /// and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose( bool disposing )
        {
            if( disposing )
            {
                _timer?.Dispose( );
            }
        }
    }
}