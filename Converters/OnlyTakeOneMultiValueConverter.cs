
namespace Booger
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class OnlyTakeOneMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 0)
                throw new ArgumentException("Not enough values");

            object _value = values[0];
            if (_value == null)
                throw new ArgumentNullException("value");

            if (!targetType.IsAssignableFrom(_value.GetType()))
                throw new ArgumentException("Not assignable");

            return _value;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Type _sourceType = value.GetType();

            foreach (var _targetType in targetTypes)
                if (!_targetType.IsAssignableFrom(_sourceType))
                    throw new ArgumentException("Not assignable");

            object[] _result = new object[targetTypes.Length];
            Array.Fill(_result, value);

            return _result;
        }
    }
}
