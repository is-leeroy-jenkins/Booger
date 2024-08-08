// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="ConditionalControl.cs" company="Terry D. Eppler">
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
//   ConditionalControl.cs
// </summary>
// ******************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Booger
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:OpenGptChat.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:OpenGptChat.Controls;assembly=OpenGptChat.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ConditionalControl/>
    ///
    /// </summary>
    public class ConditionalControl : Control
    {
        static ConditionalControl( )
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof( ConditionalControl ),
                new FrameworkPropertyMetadata( typeof( ConditionalControl ) ) );
        }

        public bool _condition
        {
            get { return (bool)GetValue( _conditionProperty ); }
            set { SetValue( _conditionProperty, value ); }
        }

        public FrameworkElement _elementWhileTrue
        {
            get
            {
                return (FrameworkElement)GetValue( _elementWhileTrueProperty );
            }
            set { SetValue( _elementWhileTrueProperty, value ); }
        }

        public FrameworkElement _elementWhileFalse
        {
            get
            {
                return (FrameworkElement)GetValue( _elementWhileFalseProperty );
            }
            set { SetValue( _elementWhileFalseProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for Condition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _conditionProperty =
            DependencyProperty.Register( nameof( _condition ), typeof( bool ),
                typeof( ConditionalControl ), new PropertyMetadata( true ) );

        // Using a DependencyProperty as the backing store for ElementWhileTrue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _elementWhileTrueProperty =
            DependencyProperty.Register( nameof( _elementWhileTrue ),
                typeof( FrameworkElement ), typeof( ConditionalControl ),
                new PropertyMetadata( null ) );

        // Using a DependencyProperty as the backing store for ElementWhileFalse.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _elementWhileFalseProperty =
            DependencyProperty.Register( nameof( _elementWhileFalse ),
                typeof( FrameworkElement ), typeof( ConditionalControl ),
                new PropertyMetadata( null ) );
    }
}