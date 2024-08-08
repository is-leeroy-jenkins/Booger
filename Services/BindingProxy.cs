
namespace Booger
{
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
            get { return (object)GetValue(BindingProxy._dataProperty); }
            set { SetValue(BindingProxy._dataProperty, value); }
        }

        public static readonly DependencyProperty _dataProperty =
            DependencyProperty.Register("_data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }
}
