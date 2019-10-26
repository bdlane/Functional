using System;

namespace Functional
{
    public interface IMaybe<out T>
    {
        TResult Match<TResult>(TResult nothing, Func<T, TResult> just);
    }    
}
