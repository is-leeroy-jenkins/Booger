// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="NativeMethods.cs" company="Terry D. Eppler">
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
//   NativeMethods.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public static class NativeMethods
    {
        /// <summary>
        /// 
        /// </summary>
        public enum DwmWindowAttribute : uint
        {
            /// <summary>
            /// The nc rendering enabled
            /// </summary>
            NcRenderingEnabled = 1,

            /// <summary>
            /// The nc rendering policy
            /// </summary>
            NcRenderingPolicy,

            /// <summary>
            /// The transitions force disabled
            /// </summary>
            TransitionsForceDisabled,

            /// <summary>
            /// The allow nc paint
            /// </summary>
            AllowNcPaint,

            /// <summary>
            /// The caption button bounds
            /// </summary>
            CaptionButtonBounds,

            /// <summary>
            /// The non client RTL layout
            /// </summary>
            NonClientRtlLayout,

            /// <summary>
            /// The force iconic representation
            /// </summary>
            ForceIconicRepresentation,

            /// <summary>
            /// The flip3 d policy
            /// </summary>
            Flip3DPolicy,

            /// <summary>
            /// The extended frame bounds
            /// </summary>
            ExtendedFrameBounds,

            /// <summary>
            /// The has iconic bitmap
            /// </summary>
            HasIconicBitmap,

            /// <summary>
            /// The disallow peek
            /// </summary>
            DisallowPeek,

            /// <summary>
            /// The excluded from peek
            /// </summary>
            ExcludedFromPeek,

            /// <summary>
            /// The cloak
            /// </summary>
            Cloak,

            /// <summary>
            /// The cloaked
            /// </summary>
            Cloaked,

            /// <summary>
            /// The freeze representation
            /// </summary>
            FreezeRepresentation,

            /// <summary>
            /// The passive update mode
            /// </summary>
            PassiveUpdateMode,

            /// <summary>
            /// The use host backdrop brush
            /// </summary>
            UseHostBackdropBrush,

            /// <summary>
            /// The use immersive dark mode
            /// </summary>
            UseImmersiveDarkMode = 20,

            /// <summary>
            /// The window corner preference
            /// </summary>
            WindowCornerPreference = 33,

            /// <summary>
            /// The border color
            /// </summary>
            BorderColor,

            /// <summary>
            /// The caption color
            /// </summary>
            CaptionColor,

            /// <summary>
            /// The text color
            /// </summary>
            TextColor,

            /// <summary>
            /// The visible frame border thickness
            /// </summary>
            VisibleFrameBorderThickness,

            /// <summary>
            /// The system backdrop type
            /// </summary>
            SystemBackdropType,

            /// <summary>
            /// The last
            /// </summary>
            Last
        }

        /// <summary>
        /// DWMs the set window attribute.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="attr">The attribute.</param>
        /// <param name="attrValue">The attribute value.</param>
        /// <param name="attrSize">Size of the attribute.</param>
        /// <returns></returns>
        [ DllImport( "dwmapi.dll", PreserveSig = true ) ]
        public static extern int DwmSetWindowAttribute( IntPtr hwnd, DwmWindowAttribute attr,
            ref int attrValue, int attrSize );

        /// <summary>
        /// Enables the dark mode for window.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="enable">if set to <c>true</c> [enable].</param>
        /// <returns></returns>
        public static bool EnableDarkModeForWindow( IntPtr hWnd, bool enable )
        {
            var _darkMode = enable
                ? 1
                : 0;

            var _hr = NativeMethods.DwmSetWindowAttribute( hWnd,
                DwmWindowAttribute.UseImmersiveDarkMode, ref _darkMode, sizeof( int ) );

            return _hr >= 0;
        }

        /// <summary>
        /// Messages the box.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        [ DllImport( "User32.dll", EntryPoint = "MessageBoxW",
            ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true ) ]
        public extern static MessageBoxResult MessageBox( IntPtr hwnd, string text, string caption,
            MessageBoxFlags flags );
    }

    /// <summary>
    /// 
    /// </summary>
    public enum MessageBoxResult
    {
        /// <summary>
        /// The ok
        /// </summary>
        Ok = 1,

        /// <summary>
        /// The cancel
        /// </summary>
        Cancel = 2,

        /// <summary>
        /// The abort
        /// </summary>
        Abort = 3,

        /// <summary>
        /// The retry
        /// </summary>
        Retry = 4,

        /// <summary>
        /// The ignore
        /// </summary>
        Ignore = 5,

        /// <summary>
        /// The yes
        /// </summary>
        Yes = 6,

        /// <summary>
        /// The no
        /// </summary>
        No = 7,

        /// <summary>
        /// The close
        /// </summary>
        Close = 8,

        /// <summary>
        /// The help
        /// </summary>
        Help = 9,

        /// <summary>
        /// The try again
        /// </summary>
        TryAgain = 10,

        /// <summary>
        /// The continue
        /// </summary>
        Continue = 11,

        /// <summary>
        /// The timeout
        /// </summary>
        Timeout = 32000
    }

    /// <summary>
    /// 
    /// </summary>
    public enum MessageBoxFlags : uint
    {
        /// <summary>
        /// The abort retry ignore
        /// </summary>
        AbortRetryIgnore = 0x2,

        /// <summary>
        /// The cancel try continue
        /// </summary>
        CancelTryContinue = 0x6,

        /// <summary>
        /// The help
        /// </summary>
        Help = 0x4000,

        /// <summary>
        /// The ok
        /// </summary>
        Ok = 0x0,

        /// <summary>
        /// The ok cancel
        /// </summary>
        OkCancel = 0x1,

        /// <summary>
        /// The retry cancel
        /// </summary>
        RetryCancel = 0x5,

        /// <summary>
        /// The yes no
        /// </summary>
        YesNo = 0x4,

        /// <summary>
        /// The yes no cancel
        /// </summary>
        YesNoCancel = 0x3,

        /// <summary>
        /// The icon exclamation
        /// </summary>
        IconExclamation = 0x30,

        /// <summary>
        /// The icon warning
        /// </summary>
        IconWarning = 0x30,

        /// <summary>
        /// The icon information
        /// </summary>
        IconInformation = 0x40,

        /// <summary>
        /// The icon asterisk
        /// </summary>
        IconAsterisk = 0x40,

        /// <summary>
        /// The icon question
        /// </summary>
        IconQuestion = 0x20,

        /// <summary>
        /// The icon stop
        /// </summary>
        IconStop = 0x10,

        /// <summary>
        /// The icon error
        /// </summary>
        IconError = 0x10,

        /// <summary>
        /// The icon hand
        /// </summary>
        IconHand = 0x10,

        /// <summary>
        /// The definition button1
        /// </summary>
        DefButton1 = 0x0,

        /// <summary>
        /// The definition button2
        /// </summary>
        DefButton2 = 0x100,

        /// <summary>
        /// The definition button3
        /// </summary>
        DefButton3 = 0x200,

        /// <summary>
        /// The definition button4
        /// </summary>
        DefButton4 = 0x300,

        /// <summary>
        /// The appl modal
        /// </summary>
        ApplModal = 0x0,

        /// <summary>
        /// The system modal
        /// </summary>
        SystemModal = 0x1000,

        /// <summary>
        /// The task modal
        /// </summary>
        TaskModal = 0x2000,

        /// <summary>
        /// The default desktop only
        /// </summary>
        DefaultDesktopOnly = 0x20000,

        /// <summary>
        /// The right
        /// </summary>
        Right = 0x80000,

        /// <summary>
        /// The RTL reading
        /// </summary>
        RtlReading = 0x100000,

        /// <summary>
        /// The set foreground
        /// </summary>
        SetForeground = 0x10000,

        /// <summary>
        /// The topmost
        /// </summary>
        Topmost = 0x40000,

        /// <summary>
        /// The service notification
        /// </summary>
        ServiceNotification = 0x200000
    }
}