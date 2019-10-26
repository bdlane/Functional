using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;
using System.Linq;

namespace Functional.FluentAssertions
{
    public static class MaybeAssertionExtensions
    {
        public static MaybeAssertions<T> Should<T>(this IMaybe<T> maybe)
        {
            if (maybe is null)
            {
                throw new ArgumentNullException(nameof(maybe));
            }

            return new MaybeAssertions<T>(maybe);
        }
    }

    // Inheriting from ObjectAssertions means we get BeEquivalentTo... etc. for free.
    // Do we want all the methods on ObjectAssertions?
    public class MaybeAssertions<T> : ObjectAssertions
    {
        public MaybeAssertions(IMaybe<T> maybe) : base(maybe)
        {
            if (maybe is null)
            {
                throw new ArgumentNullException(nameof(maybe));
            }

            Subject = maybe;
        }

        public new IMaybe<T> Subject { get; protected set; }

        protected override string Identifier => "maybe";

        public AndConstraint<MaybeAssertions<T>> BeEmpty(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.Match(true, _ => false))
                .FailWith("Expected {context:subject} to be empty, but it was filled.");

            return new AndConstraint<MaybeAssertions<T>>(this);
        }

        public AndWhichConstraint<MaybeAssertions<T>, T> BeSomething(string because = "", params object[] becauseArgs)
        {
            var value = Subject.Match(Enumerable.Empty<T>(), v => new T[] { v });

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(value.Any())
                .FailWith("Expected {context:subject} to be something, but it was empty.");

            return new AndWhichConstraint<MaybeAssertions<T>, T>(this, value.Single());
        }
    }
}
