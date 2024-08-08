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
        public static bool Get_isAutoLogicFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(FocusUtils._isAutoLogicFocusProperty);
        }

        public static void Set_isAutoLogicFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(FocusUtils._isAutoLogicFocusProperty, value);
        }

        public static bool Get_isAutoKeyboardFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(FocusUtils._isAutoKeyboardFocusProperty);
        }

        public static void Set_isAutoKeyboardFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(FocusUtils._isAutoKeyboardFocusProperty, value);
        }

        public static readonly DependencyProperty _isAutoLogicFocusProperty =
            DependencyProperty.RegisterAttached("_isAutoLogicFocus", typeof(bool), typeof(FocusUtils), new PropertyMetadata(false, PropertyIsAutoLogicFocusChanged));

        public static readonly DependencyProperty _isAutoKeyboardFocusProperty =
            DependencyProperty.RegisterAttached("_isAutoKeyboardFocus", typeof(bool), typeof(FocusUtils), new PropertyMetadata(false, PropertyIsAutoKeyboardFocusChanged));


        private static void PropertyIsAutoLogicFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement _element)
                throw new InvalidOperationException("You can only attach this property to FrameworkElement.");

            RoutedEventHandler _loaded = (s, e) => _element.Focus();

            if (e.NewValue is bool _b && _b)
                _element.Loaded += _loaded;
            else
                _element.Loaded -= _loaded;
        }
        private static void PropertyIsAutoKeyboardFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement _element)
                throw new InvalidOperationException("You can only attach this property to FrameworkElement.");

            RoutedEventHandler _loaded = (s, e) => Keyboard.Focus(_element);

            if (e.NewValue is bool _b && _b)
                _element.Loaded += _loaded;
            else
                _element.Loaded -= _loaded;
        }
    }
}
