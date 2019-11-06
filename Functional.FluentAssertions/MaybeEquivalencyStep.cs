using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using System;
using System.Linq;
using System.Reflection;

namespace Functional.FluentAssertions
{
    public class MaybeEquivalencyStep : IEquivalencyStep
    {
        private static readonly MethodInfo HandleMethod = new Action<IMaybe<object>, IMaybe<object>, IEquivalencyValidationContext, IEquivalencyValidator>(HandleCore)
            .GetMethodInfo().GetGenericMethodDefinition();


        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            var expectationType = config.GetExpectationType(context); ;

            return IsMaybe(expectationType);
        }

        /// <summary>
        /// Applies a step as part of the task to compare two objects for structural equality.
        /// </summary>
        /// <value>
        /// Should return <c>true</c> if the subject matches the expectation or if no additional assertions
        /// have to be executed. Should return <c>false</c> otherwise.
        /// </value>
        /// <remarks>
        /// May throw when preconditions are not met or if it detects mismatching data.
        /// </remarks>
        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            if (!(AssertSubjectIsMaybe(context.Subject)))
            {
                return true;
            }

            if (!(AssertExpectationIsNotNull(context.Expectation, context.Subject)))
            {
                return true;
            }

            var expectationEnclosedType = config.GetExpectationType(context).GetGenericArguments().Single();
            var subjectEnclosedType = context.Subject.GetType().GetGenericArguments().Single(); // This gets the runtime type. Declard type config?

            try
            {
                HandleMethod.MakeGenericMethod(subjectEnclosedType, expectationEnclosedType)
                    .Invoke(null, new[] { context.Subject, context.Expectation, context, parent });
            }
            catch (TargetInvocationException e)
            {
                throw e;//.Unwrap();
            }



            return true;
        }

        private static void HandleCore<TSubject, TExpectation>(IMaybe<TSubject> subject, IMaybe<TExpectation> expectation, IEquivalencyValidationContext context, IEquivalencyValidator parent)
        {
            expectation.Match(none, some)();

            void none()
            {
                subject.Match(none: () => { },
                              some: _ => new Action(() => AssertionScope.Current.FailWith("Expected subject to be empty, but it was filled.")))();
            }

            void some(TExpectation e)
            {
                subject.Match(none: () => AssertionScope.Current.FailWith("Expected {context:subject} to be filled, but it was empty."),
                              some: (s) => new Action(() => parent.AssertEqualityUsing(context.CreateForMaybeValue(s, e))))();
            }
        }

        private static bool IsMaybe(Type type) =>
            type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IMaybe<>) ||
            type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMaybe<>));

        private static bool AssertSubjectIsMaybe(object subject)
        {
            bool conditionMet = AssertionScope.Current
                .ForCondition(!(subject is null))
                .FailWith("Expected {context:subject} not to be {0}.", new object[] { null });

            if (conditionMet)
            {
                conditionMet = AssertionScope.Current
                    .ForCondition(IsMaybe(subject.GetType()))
                    .FailWith($"Expected {{context:subject}} to be a {typeof(IMaybe<>)}, but it was a {{0}}.", subject.GetType());
            }

            return conditionMet;
        }

        // Validator?
        private static bool AssertExpectationIsNotNull(object expectation, object subject)
        {
            return AssertionScope.Current
                .ForCondition(!(expectation is null))
                .FailWith("Expected {context:subject} to be <null>, but found {0}.", new object[] { subject });
        }


    }

    internal static class EquivalencyValidationContextExtensions
    {
        public static IEquivalencyValidationContext CreateForMaybeValue<TSubject, TExpectation>(this IEquivalencyValidationContext context, TSubject subject, TExpectation expectation)
        {
            return new EquivalencyValidationContext
            {
                SelectedMemberInfo = context.SelectedMemberInfo,
                Subject = subject,
                Expectation = expectation,
                SelectedMemberPath = context.SelectedMemberPath,
                SelectedMemberDescription = context.SelectedMemberDescription,
                Because = context.Because,
                BecauseArgs = context.BecauseArgs,
                CompileTimeType = typeof(TExpectation),
                RootIsCollection = context.RootIsCollection,
                Tracer = context.Tracer
            };
        }
    }
}
