using FluentAssertions;
using System;
using Xunit;
using Xunit.Sdk;

namespace Functional.FluentAssertions.Tests
{
    public class MaybeAssertionsTests
    {
        [Fact]
        public void CanAssertAMaybeIsEmpty()
        {
            // Arrange
            var subject = new Nothing<object>();

            // Act
            Action act = () => subject.Should().BeEmpty();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void AssertionFailsWhenAssertingAFilledMaybeIsEmpty()
        {
            // Arrange
            var subject = new Just<object>(new object());

            // Act
            Action act = () => subject.Should().BeEmpty();

            // Assert
            act.Should().Throw<XunitException>("Expected subject to be empty, but it was something.");
        }

        [Fact]
        public void AssertionFailsWhenAssertingAnEmptyMaybeIsFilled()
        {
            // Arrange
            var subject = new Nothing<object>();

            // Act
            Action act = () => subject.Should().BeSomething();

            // Assert
            act.Should().Throw<XunitException>("Expected subject to be something, but it was empty.");
        }

        [Fact]
        public void CanAssertAMabyeIsFilled()
        {
            // Arrange
            var value = Guid.NewGuid();
            var subject = new Just<Guid>(value);

            // Act
            Func<Guid> fun = () => subject.Should().BeSomething().Subject;

            // Assert
            fun.Should().NotThrow().Which.Should().Be(value);
        }
    }
}
