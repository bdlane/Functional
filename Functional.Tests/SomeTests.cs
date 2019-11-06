using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using Xunit;

namespace Functional.Tests
{
    public class SomeTests
    {
        [Fact]
        public void CannotCreateWithNull()
        {
            // Arrange

            // Act
            Action act = () => new Some<object>(null);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [AutoData]
        public void MatchInvokesSome(int x, int y)
        {
            // Arrange
            var sut = new Some<int>(x);
            var expected = x + y;

            // Act
            var actual = sut.Match(-1, i => i + y);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void SomeArgumentCannotBeNull()
        {
            // Arrange
            var sut = new Some<Guid>(Guid.NewGuid());

            // Act
            Action act = () => sut.Match("", null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void SomeArgumentCannotReturnNull()
        {
            // Arrange
            var sut = new Some<Guid>(Guid.NewGuid());

            // Act
            Action act = () => sut.Match(null, _ => (string)null);

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
