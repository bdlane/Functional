using System;

namespace Functional
{
    public class Just<T> : IMaybe<T>
    {
        private readonly T value;

        public Just(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.value = value;
        }

        public TResult Match<TResult>(TResult nothing, Func<T, TResult> just)
        {
            if (just == null)
            {
                throw new ArgumentNullException(nameof(just));
            }

            var result = just(value);

            if (result == null)
            {
                 throw new ArgumentException("Expression cannot return null.", nameof(just));
            }

            return just(value);
        }
    }
}
