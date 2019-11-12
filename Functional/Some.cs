using System;

namespace Functional
{
    public class Some<T> : IMaybe<T>, IEquatable<Some<T>>
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

        public override bool Equals(object obj)
        {
            if(!(obj is Some<T> s))
            {
                return false;
            }

            return Equals(s);
        }

        public override int GetHashCode() => value.GetHashCode();
        
        public bool Equals(Some<T> other)
        {
            if (other is null)
            {
                return false;
            }

            return value.Equals(other.value);
        }
    }
}
