using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using Xunit;

namespace Functional.Tests
{
    public class MaybeTests
    {
        [Fact]
        public void BindReturnsEmptyWhenSourceIsEmpty()
        {
            // Arrange
            var sut = new Nothing<Guid>();

            // Act
            var actual = sut.Bind(_ => Guid.NewGuid());

            // Assert
            actual.Match(true, _ => false).Should().BeTrue();
        }

        [Theory]
        [InlineAutoData]
        public void BindReturnsResultOfSelectorWhenSourceIsFilled(int x, int y)
        {
            // Arrange
            var sut = new Just<int>(x);
            var expected = x + y;

            // Act
            var actual = sut.Bind(_ => x);

            // Assert
            actual.Match(0, i => i + y).Should().Be(expected);
        }

        // Match -> return Action
        // When -> executes Action

        [Theory]
        [InlineAutoData]
        public void MatchExecutesNothingAction(int actual, int expected)
        {
            // Arrange
            var sut = new Nothing<int>();

            // Act
            sut.Match(() => actual = expected, (v) => actual = v)();

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineAutoData]
        public void MatchExecutesJustAction(int actual, int expected)
        {
            // Arrange
            var sut = new Just<int>(expected);

            // Act
            sut.Match(() => actual = 0, (v) => actual = v)();

            // Assert
            actual.Should().Be(expected);
        }
    }
}
