using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Equivalency;
using System;
using Xunit;
using Xunit.Sdk;

namespace Functional.FluentAssertions.Tests
{
    public class MaybeEquivalencyStepTests
    {
        public MaybeEquivalencyStepTests()
        {
            AssertionOptions.EquivalencySteps.Reset();
        }

        [Fact]
        public void CanHandleNothingExpectation()
        {
            // Arrange
            var expectation = new None<int>();

            var context = new EquivalencyValidationContext
            {
                Expectation = expectation,
                CompileTimeType = expectation.GetType()
            };

            var config = new EquivalencyAssertionOptions<None<int>>();

            var sut = new MaybeEquivalencyStep();

            // Act
            var actual = sut.CanHandle(context, config);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void CanHandleJustExpectation()
        {
            // Arrange
            var expectation = new Some<Guid>(Guid.NewGuid());

            var context = new EquivalencyValidationContext
            {
                Expectation = expectation,
                CompileTimeType = expectation.GetType()
            };

            var config = new EquivalencyAssertionOptions<None<int>>();

            var sut = new MaybeEquivalencyStep();

            // Act
            var actual = sut.CanHandle(context, config);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void CanHandleIMaybeExpectation()
        {
            // Arrange
            IMaybe<Guid> expectation = new Some<Guid>(Guid.NewGuid());

            var context = new EquivalencyValidationContext
            {
                Expectation = expectation,
                CompileTimeType = expectation.GetType()
            };

            var config = new EquivalencyAssertionOptions<None<int>>();

            var sut = new MaybeEquivalencyStep();

            // Act
            var actual = sut.CanHandle(context, config);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void CannotHandleObjectExpectation()
        {
            // Arrange
            object expectation = new object();

            var context = new EquivalencyValidationContext
            {
                Expectation = expectation,
                CompileTimeType = expectation.GetType()
            };

            var config = new EquivalencyAssertionOptions<None<int>>();

            var sut = new MaybeEquivalencyStep();

            // Act
            var actual = sut.CanHandle(context, config);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public void AssertionFailsWhenSubjectIsNull()
        {
            // Arrange
            object subject = null;

            AssertionOptions.EquivalencySteps.AddAfter<ReferenceEqualityEquivalencyStep, MaybeEquivalencyStep>();

            var sut = new MaybeEquivalencyStep();

            // Act
            Action act = () => subject.Should().BeEquivalentTo(new None<Guid>());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject not to be <null>.*");
        }

        [Fact]
        public void AssertionFailsWhenSubjectIsNotAMaybe()
        {
            // Arrange
            object subject = new object();

            AssertionOptions.EquivalencySteps.AddAfter<ReferenceEqualityEquivalencyStep, MaybeEquivalencyStep>();

            var sut = new MaybeEquivalencyStep();

            // Act
            Action act = () => subject.Should().BeEquivalentTo(new None<Guid>());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"Expected subject to be a {typeof(IMaybe<>)}, but it was a {typeof(object)}.*"); ;
        }

        [Fact]
        public void AssertionFailsWhenExpectationIsNull()
        {
            // Arrange
            var subject = new None<object>();

            AssertionOptions.EquivalencySteps.AddAfter<ReferenceEqualityEquivalencyStep, MaybeEquivalencyStep>();

            var sut = new MaybeEquivalencyStep();

            // Act
            Action act = () => subject.Should().BeEquivalentTo((IMaybe<object>)null);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"Expected subject to be <null>, but found *Functional.Nothing*");
        }

        [Fact]
        public void AssertionFailsWhenSubjectIsEmptyAndExpectationIsFilled()
        {
            // Arrange
            var subject = new None<object>();
            var expectation = new Some<object>(new object());

            AssertionOptions.EquivalencySteps.Remove<IEquivalencyStep>();
            AssertionOptions.EquivalencySteps.Insert<MaybeEquivalencyStep>();

            var sut = new MaybeEquivalencyStep();

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected subject to be filled, but it was empty.*");
        }

        [Fact]
        public void AssertionFailsWhenSubjectIsFilledAndExpectationIsEmpty()
        {
            // Arrange
            var subject = new Some<object>(new object());
            var expectation = new None<object>();

            AssertionOptions.EquivalencySteps.Remove<IEquivalencyStep>();
            AssertionOptions.EquivalencySteps.Insert<MaybeEquivalencyStep>();

            var sut = new MaybeEquivalencyStep();

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected subject to be empty, but it was filled.*");
        }

        [Fact]
        public void CanAssertAnEmptySubjectAndEmptyExpectationAreEqual()
        {
            // Arrange
            var subject = new None<object>();
            var expectation = new None<object>();

            AssertionOptions.EquivalencySteps.Remove<IEquivalencyStep>();
            AssertionOptions.EquivalencySteps.Insert<MaybeEquivalencyStep>();

            var sut = new MaybeEquivalencyStep();

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void CanAssertAEmptyMaybesOfIncompatibleTypesAreEqual()
        {
            // Arrange
            var subject = new None<int>();
            var expectation = new None<string>();

            AssertionOptions.EquivalencySteps.Remove<IEquivalencyStep>();
            AssertionOptions.EquivalencySteps.Insert<MaybeEquivalencyStep>();

            var sut = new MaybeEquivalencyStep();

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [AutoData]
        public void AssertionFailsWhenContainedValuesAreNotEquivalent(int subjectValue, int expectationValue)
        {
            // Arrange
            var subject = new Some<int>(subjectValue);
            var expectation = new Some<int>(expectationValue);

            AssertionOptions.EquivalencySteps.Insert<MaybeEquivalencyStep>();

            var sut = new MaybeEquivalencyStep();

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().Throw<XunitException>().WithMessage($"Expected object to be*{expectationValue}, but *");
        }

        [Theory]
        [AutoData]
        public void AssertionFailsWhenContainedObjectsAreNotEquivalent(Foo subjectValue, Foo expectationValue)
        {
            // Arrange
            var subject = new Some<Foo>(subjectValue);
            var expectation = new Some<Foo>(expectationValue);

            AssertionOptions.EquivalencySteps.Insert<MaybeEquivalencyStep>();

            var sut = new MaybeEquivalencyStep();

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().Throw<XunitException>().WithMessage($"Expected member Name to be*\"{expectationValue.Name}\", but *");
        }

        [Theory]
        [AutoData]
        public void CanAssertMaybesContainingEquivalentValuesAreEquivalent(int value)
        {
            // Arrange
            var subject = new Some<int>(value);
            var expectation = new Some<int>(value);

            AssertionOptions.EquivalencySteps.Insert<MaybeEquivalencyStep>();

            var sut = new MaybeEquivalencyStep();

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [AutoData]
        public void CanAssertMaybesContainingEquivalentObjectsAreEquivalent(Foo value)
        {
            // Arrange
            var subject = new Some<Foo>(value);
            var expectation = new Some<Foo>(value);

            AssertionOptions.EquivalencySteps.Insert<MaybeEquivalencyStep>();

            var sut = new MaybeEquivalencyStep();

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [AutoData]
        public void AssertionFailsWhenContainedObjectsHaveMaybePropertiesAndAreNotEquivalent(int id, string subjectName,  string expectationName)
        {
            // Arrange
            var subject = new Some<Bar>(new Bar { Id = id, Foo = new Some<Foo>(new Foo { Name = subjectName }) });
            var expectation = new Some<Bar>(new Bar { Id = id, Foo = new Some<Foo>(new Foo { Name = expectationName }) });

            AssertionOptions.EquivalencySteps.Insert<MaybeEquivalencyStep>();

            var sut = new MaybeEquivalencyStep();

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().Throw<XunitException>().WithMessage($"Expected member Foo.Name to be *{expectationName}*but*{subjectName}*");
        }

        [Theory]
        [AutoData]
        public void CanAssertMaybesContainingEquivalentObjectsWithMaybePropertiesAreEquivalent(int id, string name)
        {
            // Arrange
            var subject = new Some<Bar>(new Bar { Id = id, Foo = new Some<Foo>(new Foo { Name = name }) });
            var expectation = new Some<Bar>(new Bar { Id = id, Foo = new Some<Foo>(new Foo { Name = name }) });

            AssertionOptions.EquivalencySteps.Insert<MaybeEquivalencyStep>();

            var sut = new MaybeEquivalencyStep();

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().NotThrow();
        }
    }

    public class Foo
    {
        public string Name { get; set; }
    }

    public class Bar
    {
        public int Id { get; set; }

        public IMaybe<Foo> Foo { get; set; }
    }
}
