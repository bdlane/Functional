using System;

namespace Functional
{
    public class None<T> : IMaybe<T>
    {
        public TResult Match<TResult>(TResult none, Func<T, TResult> some)
        {
            if (none == null)
            {
                throw new ArgumentNullException(nameof(none));
            }

            return none;
        }
    }
}
