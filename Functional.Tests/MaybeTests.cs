using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using Xunit;
using Functional.FluentAssertions;

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

        [Theory]
        [InlineAutoData]
        public void FromReturnsJustWhenValueIsNotNull(object value)
        {
            // Arrange

            // Act
            var actual = Maybe.From(value);

            // Assert
            actual.Match(new object(), v => v).Should().Be(value);
        }

        [Fact]
        public void FromReturnsNothingWhenValueIsNull()
        {
            // Arrange

            // Act
            var actual = Maybe.From((object)null);

            // Assert
            actual.Should().BeEmpty();
        }

        [Theory]
        [InlineAutoData]
        public void FromWithPredicateReturnsJustWhenValueIsNotNullAndPredicateIsTrue(object value)
        {
            // Arrange

            // Act
            var actual = Maybe.From(value, v => v == value);

            // Assert
            actual.Match(new object(), v => v).Should().Be(value);
        }

        [Theory]
        [InlineAutoData]
        public void FromWithPredicateReturnsNothingWhenValueIsNotNullAndPredicateIsFalse(object value)
        {
            // Arrange

            // Act
            var actual = Maybe.From(value, _ => false);

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public void FromWithPredicateReturnsNothingWhenValueIsNullAndPredicateIsTrue()
        {
            // Arrange

            // Act
            var actual = Maybe.From((object)null, _ => true);

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public void FromIsNullOrEmptyReturnsNothingWhenStringIsNull()
        {
            // Arrange

            // Act
            var actual = Maybe.FromIsNullOrEmpty(null);

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public void FromIsNullOrEmptyReturnsNothingWhenStringIsEmpty()
        {
            // Arrange

            // Act
            var actual = Maybe.FromIsNullOrEmpty("");

            // Assert
            actual.Should().BeEmpty();
        }

        [Theory]
        [InlineAutoData]
        public void FromIsNullOrEmptyReturnsFilledWhenStringIsNotEmpty(string s)
        {
            // Arrange

            // Act
            var actual = Maybe.FromIsNullOrEmpty(s);

            // Assert
            actual.Should().BeSomething();
        }

        [Fact]
        public void EmptyReturnsNothing()
        {
            // Arrange

            // Act

            // Assert
            Maybe.Empty<object>().Should().BeEmpty();
        }
    }
}
