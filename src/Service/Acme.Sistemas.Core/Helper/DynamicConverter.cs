using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;

namespace Acme.Sistemas.Core.Helper;

public static class DynamicConverter
{
    private static readonly ConcurrentDictionary<Type, TypeConverter> ConverterCache = new();

    public static T? To<T>(object? value)
    {
        if (value is null || value is DBNull) return default;
        if (value is T typed) return typed;

        var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
        var converter = ConverterCache.GetOrAdd(targetType, TypeDescriptor.GetConverter);

        if (converter.CanConvertFrom(value.GetType()))
            return (T?)converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

        return (T?)Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
    }

    public static object? To(object? value, Type targetType)
    {
        if (value is null || value is DBNull) return null;
        if (targetType.IsInstanceOfType(value)) return value;

        var underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;
        var converter = ConverterCache.GetOrAdd(underlying, TypeDescriptor.GetConverter);

        if (converter.CanConvertFrom(value.GetType()))
            return converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

        return Convert.ChangeType(value, underlying, CultureInfo.InvariantCulture);
    }
}
