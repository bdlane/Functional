using AutoFixture.Kernel;
using System;
using System.Linq;

namespace Functional.AutoFixture
{
    public class MaybeSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (!(request is Type t))
            {
                return new NoSpecimen();
            }

            if (!t.IsInterface || !t.IsGenericType || t.GetGenericTypeDefinition() != typeof(IMaybe<>))
            {
                return new NoSpecimen();
            }

            var containedType = t.GetGenericArguments().First();
            var someType = typeof(Some<>).MakeGenericType(containedType);

            return context.Resolve(someType);
        }
    }
}
