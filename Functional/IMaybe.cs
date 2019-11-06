using System;

namespace Functional
{
    public interface IMaybe<out T>
    {
        TResult Match<TResult>(TResult none, Func<T, TResult> some);
    }    
}
