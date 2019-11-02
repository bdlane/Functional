using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using Xunit;

namespace Functional.Tests
{
    public class JustTests
    {
        [Fact]
        public void CannotCreateWithNull()
        {
            // Arrange

            // Act
            Action act = () => new Just<object>(null);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [AutoData]
        public void MatchInvokesJust(int x, int y)
        {
            // Arrange
            var sut = new Just<int>(x);
            var expected = x + y;

            // Act
            var actual = sut.Match(-1, i => i + y);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void JustArgumentCannotBeNull()
        {
            // Arrange
            var sut = new Just<Guid>(Guid.NewGuid());

            // Act
            Action act = () => sut.Match("", null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void JustArgumentCannotReturnNull()
        {
            // Arrange
            var sut = new Just<Guid>(Guid.NewGuid());

            // Act
            Action act = () => sut.Match(null, _ => (string)null);

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
