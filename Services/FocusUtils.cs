namespace Booger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    public class FocusUtils : FrameworkElement
    {
        public static bool GetIsAutoLogicFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsAutoLogicFocusProperty);
        }

        public static void SetIsAutoLogicFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(IsAutoLogicFocusProperty, value);
        }

        public static bool GetIsAutoKeyboardFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsAutoKeyboardFocusProperty);
        }

        public static void SetIsAutoKeyboardFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(IsAutoKeyboardFocusProperty, value);
        }

        public static readonly DependencyProperty IsAutoLogicFocusProperty =
            DependencyProperty.RegisterAttached("IsAutoLogicFocus", typeof(bool), typeof(FocusUtils), new PropertyMetadata(false, PropertyIsAutoLogicFocusChanged));

        public static readonly DependencyProperty IsAutoKeyboardFocusProperty =
            DependencyProperty.RegisterAttached("IsAutoKeyboardFocus", typeof(bool), typeof(FocusUtils), new PropertyMetadata(false, PropertyIsAutoKeyboardFocusChanged));


        private static void PropertyIsAutoLogicFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement element)
                throw new InvalidOperationException("You can only attach this property to FrameworkElement.");

            RoutedEventHandler loaded = (s, e) => element.Focus();

            if (e.NewValue is bool b && b)
                element.Loaded += loaded;
            else
                element.Loaded -= loaded;
        }
        private static void PropertyIsAutoKeyboardFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement element)
                throw new InvalidOperationException("You can only attach this property to FrameworkElement.");

            RoutedEventHandler loaded = (s, e) => Keyboard.Focus(element);

            if (e.NewValue is bool b && b)
                element.Loaded += loaded;
            else
                element.Loaded -= loaded;
        }
    }
}
