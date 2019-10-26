using System;

namespace Functional
{
    public class Nothing<T> : IMaybe<T>
    {
        public TResult Match<TResult>(TResult nothing, Func<T, TResult> just)
        {
            if (nothing == null)
            {
                throw new ArgumentNullException(nameof(nothing));
            }

            return nothing;
        }
    }
}
