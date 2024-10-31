

namespace Booger
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public static class ValueWrapperExtensions
    {
        public static ValueWrapper<T> Wrap<T>(this T item) => new ValueWrapper<T>(item);

        public static ObservableCollection<ValueWrapper<T>> WrapCollection<T>(this IEnumerable<T> collection)
        {
            ObservableCollection<ValueWrapper<T>> _wrapped =
                new ObservableCollection<ValueWrapper<T>>();

            foreach (var _item in collection)
                _wrapped.Add(_item.Wrap());

            return _wrapped;
        }

        public static void UnwrapTo<T>(this ObservableCollection<ValueWrapper<T>> wrapped, IList<T> list)
        {
            list.Clear();

            foreach (var _item in wrapped)
                list.Add(_item.Value);
        }

        public static T[] UnwrapToArray<T>(this ObservableCollection<ValueWrapper<T>> wrapped)
        {
            return wrapped
                .Select(wrapped => wrapped.Value)
                .ToArray();
        }
    }
} 
