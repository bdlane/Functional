using System;

namespace Functional
{
    public class Some<T> : IMaybe<T>
    {
        private readonly T value;

        public Some(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.value = value;
        }

        public TResult Match<TResult>(TResult none, Func<T, TResult> some)
        {
            if (some == null)
            {
                throw new ArgumentNullException(nameof(some));
            }

            var result = some(value);

            if (result == null)
            {
                 throw new ArgumentException("Expression cannot return null.", nameof(some));
            }

            return some(value);
        }
    }
}
