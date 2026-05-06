using System.Data;
using Acme.Sistemas.Core.Helper;

namespace Acme.Sistemas.Repository.Helper;

public static class ConvertExtensions
{
    public static T? GetValueOrDefault<T>(this IDataRecord record, string columnName)
    {
        var ordinal = record.GetOrdinal(columnName);
        if (record.IsDBNull(ordinal)) return default;
        var raw = record.GetValue(ordinal);
        return DynamicConverter.To<T>(raw);
    }

    public static T? GetValueOrDefault<T>(this IDataRecord record, int ordinal)
    {
        if (record.IsDBNull(ordinal)) return default;
        var raw = record.GetValue(ordinal);
        return DynamicConverter.To<T>(raw);
    }

    public static IReadOnlyList<T> ReadAll<T>(this IDataReader reader, Func<IDataRecord, T> map)
    {
        var list = new List<T>();
        while (reader.Read())
        {
            list.Add(map(reader));
        }
        return list;
    }

    public static T? ReadFirstOrDefault<T>(this IDataReader reader, Func<IDataRecord, T> map)
    {
        return reader.Read() ? map(reader) : default;
    }
}
