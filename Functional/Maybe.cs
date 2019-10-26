using System;

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

            return source.Match<IMaybe<TResult>>(nothing: new Nothing<TResult>(),
                                                 just: v => new Just<TResult>(selector(v)));
        }

        public static Action Match<T>(this IMaybe<T> source, Action nothing, Action<T> just)
        {
            return source.Match(nothing: nothing,
                                just: (v) => () => just(v));
        }
    }
}
