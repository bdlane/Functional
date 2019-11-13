using System;
using System.Collections.Generic;
using System.Linq;

namespace Functional
{
    public static class Maybe
    {
        public static IMaybe<TResult> Bind<T, TResult>(this IMaybe<T> source, Func<T, TResult> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.Match<IMaybe<TResult>>(none: new None<TResult>(),
                                                 some: v => new Some<TResult>(selector(v)));
        }

        public static Action Match<T>(this IMaybe<T> source, Action none, Action<T> some)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Match(none: none,
                                some: (v) => () => some(v));
        }

        /// <summary>
        /// Returns the value of the specified maybe, or the specified value if the maybe is empty.
        /// </summary>
        /// <typeparam name="T">The type of the object contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The maybe to return the specified value for.</param>
        /// <param name="none">The value to return if the maybe is empty.</param>
        /// <returns></returns>
        public static T Match<T>(this IMaybe<T> source, T none)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Match(none: none, some: v => v);
        }

        public static IMaybe<T> From<T>(T value)
        {
            return value != null ? (IMaybe<T>)new Some<T>(value) : new None<T>();
        }

        public static IMaybe<T> From<T>(T value, Predicate<T> predicate)
        {
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return value != null && predicate(value) ? (IMaybe<T>)new Some<T>(value) : new None<T>();
        }

        public static IMaybe<string> FromIsNullOrEmpty(string value)
        {
            return From(value, s => !string.IsNullOrEmpty(s));
        }

        public static IMaybe<T> Empty<T>() => new None<T>();

        public static IEnumerable<T> Choose<T>(this IEnumerable<IMaybe<T>> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Select(m => m.Match(none: Enumerable.Empty<T>(),
                                              some: v => new [] { v }))
                         .SelectMany(i => i);
        }

        public static IMaybe<T> SingleOrNone<T>(this IEnumerable<IMaybe<T>> souce)
        {
            if (souce is null)
            {
                throw new ArgumentNullException(nameof(souce));
            }

            return souce.DefaultIfEmpty(new None<T>()).Single();
        }
    }
}
