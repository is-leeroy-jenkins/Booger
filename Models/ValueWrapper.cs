
namespace Booger
{
    using System;
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class ValueWrapper<T> : ObservableObject
    {
        public ValueWrapper(T value)
        {
            _value = value;
        }

        [ObservableProperty]
        private T _value;
    }
}
