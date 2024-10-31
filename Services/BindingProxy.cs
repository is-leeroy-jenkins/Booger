
namespace Booger
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object _data
        {
            get { return GetValue(_dataProperty); }
            set { SetValue(_dataProperty, value); }
        }

        public static readonly DependencyProperty _dataProperty =
            DependencyProperty.Register("_data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }
}
