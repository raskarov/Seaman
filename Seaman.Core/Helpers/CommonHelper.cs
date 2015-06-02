using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Seaman.Core
{
    /// <summary>
    /// Contains common routines
    /// </summary>
    public static class CommonHelper
    {
        /// <summary>
        /// Get value of default from dictionary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default (TValue))
        {
            if (dictionary == null)
                return defaultValue;
            TValue result;
            if (!dictionary.TryGetValue(key, out result))
                result = defaultValue;
            return result;
        }

        /// <summary>
        /// Convert string to type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T? AsT<T>(this String value)
            where T : struct
        {
            return (T?)new NullableConverter(typeof(T?)).ConvertFromInvariantString(value);
        }

        /// <summary>
        /// Convert String to type with default 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T AsT<T>(this String value, T defaultValue)
            where T : struct
        {
            return value.AsT<T>() ?? defaultValue;
        }
        /// <summary>
        /// Convert nullable value to string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String AsString<T>(this T? value)
            where T : struct
        {
            if (value == null)
                return null;
            return new NullableConverter(typeof (T?)).ConvertToInvariantString(value);
        }

        /// <summary>
        /// Make action with object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="obj"></param>
        /// <param name="setter"></param>
        /// <returns></returns>
        public static TResult Make<T, TResult>(this T obj, Func<T, TResult> setter)
        {
            return setter(obj);
        }

        /// <summary>
        /// Returns null in case source is null or equals default value for current type
        /// </summary>
        public static T? NullIfDefault<T>(this T? src)
            where T : struct
        {
            return NullIf(src, default(T));
        }
        /// <summary>
        /// Returns null in case source is null or equals provided value
        /// </summary>
        public static T? NullIf<T>(this T? src, T value)
            where T : struct
        {
            if (src == null || src.Value.Equals(value))
                return null;
            return src;
        }

        /// <summary>
        /// Return default string if initial is null or empty
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static String IfNullOrEmpty(this String obj, String defaultValue)
        {
            return String.IsNullOrEmpty(obj) ? defaultValue : obj;
        }

        /// <summary>
        /// Return processed string if initial is not null or empty
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static String IfNotNullOrEmpty(this String obj, Func<String, String> func)
        {
            return String.IsNullOrEmpty(obj) ? obj : func(obj);
        }
        /// <summary>
        /// Maps value in case it is not null or returns default value for type to which it should be mapped to.
        /// </summary>
        /// <typeparam name="T">Type to map from</typeparam>
        /// <typeparam name="TR">Type to map to</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="func">The mapping function.</param>
        /// <param name="ifNull">Default value</param>
        public static TR IfNotNull<T, TR>(this T obj, Func<T, TR> func, TR ifNull = default(TR))
        {
            return obj == null ? ifNull : func.Invoke(obj);
        }

        /// <summary>
        /// shortcut for String.IsNullOrWhiteSpace
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsNullOrWhiteSpace(this String str)
        {
            return String.IsNullOrWhiteSpace(str);
        }
        /// <summary>
        /// shortcut for String.IsNullOrEmpty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsNullOrEmpty(this String str)
        {
            return String.IsNullOrEmpty(str);
        }
        /// <summary>
        /// shortcut for format
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static String Fmt(this String str, params object[] args)
        {
            return String.Format(str, args);
        }

        /// <summary>
        /// Converts string with keys and values delimited by equals sign into
        /// <see cref="NameValueCollection"/> object
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="lowercaseKeys"></param>
        public static NameValueCollection ToNameValueCollection(this String source, Boolean lowercaseKeys = false)
        {
            var result = new NameValueCollection();
            if (String.IsNullOrWhiteSpace(source))
                return result;
            foreach (var s in source.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = s.Split(new[] { '=' }, 2);
                var key = lowercaseKeys ? parts[0].ToLowerInvariant() : parts[0];
                result[key] = parts.Length > 1 ? parts[1] : null;
            }
            return result;
        }

        /// <summary>
        /// Add item into collection if it is not already there
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Boolean EnsureContains<T>(this ICollection<T> collection, T item)
        {
            if (collection.Contains(item))
                return true;
            collection.Add(item);
            return false;
        }

        /// <summary>
        /// Get item or default from dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T TryGet<T, TK>(this IDictionary<TK, T> dict, TK key, T defaultValue = default (T))
        {
            return dict.ContainsKey(key) ? dict[key] : defaultValue;
        }

        /// <summary>
        /// Get item or add from function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="getFunc"></param>
        /// <returns></returns>
        public static T GetOrAdd<T, TK>(this IDictionary<TK, T> dict, TK key, Func<T> getFunc)
        {
            return dict.ContainsKey(key) ? dict[key] : dict[key] = getFunc();
        }

        /// <summary>
        /// Make first letter capital
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String StartWithCapital(this String str)
        {
            if (String.IsNullOrWhiteSpace(str))
                return str;
            return str.Substring(0, 1).ToUpperInvariant() + str.Substring(1);
        }

        public static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// Convert datetime to unix timestamp
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Double ToUnixTimestamp(this DateTime dt)
        {
            return dt.ToUniversalTime().Subtract(UnixEpoch).TotalSeconds;
        }
        /// <summary>
        /// Convert unix timestamp to datetime
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimestamp(this Double d)
        {
            return UnixEpoch.AddSeconds(d);
        }
        /// <summary>
        /// Convert unix timestamp to datetime
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimestamp(this Int64 d)
        {
            return UnixEpoch.AddSeconds(d);
        }

        public static Boolean IsMatching(this DateTime day, WeekDays mask)
        {
            return day.DayOfWeek.IsMatching(mask);
        }

        public static Boolean IsMatching(this DayOfWeek day, WeekDays mask)
        {
            return ((1 << (Int32)day) & (Int32)mask) != 0;
        }

        public static T GetOrSet<T, TBase>(this IDictionary<String, TBase> dict, String key, Func<T> setter)
            where T : TBase
        {
            TBase result;
            if (!dict.TryGetValue(key, out result))
            {
                result = setter();
                dict[key] = result;
            }
            return (T)result;
        }
    }
}