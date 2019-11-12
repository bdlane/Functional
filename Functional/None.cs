using System;

namespace Functional
{
    public class None<T> : IMaybe<T>, IEquatable<None<T>>
    {
        public TResult Match<TResult>(TResult none, Func<T, TResult> some)
        {
            if (none == null)
            {
                throw new ArgumentNullException(nameof(none));
            }

            return none;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is None<T> n))
            {
                return false;
            }

            return Equals(n);
        }

        public override int GetHashCode() => typeof(T).GetHashCode();

        public bool Equals(None<T> other) => other is None<T>;
    }
}
