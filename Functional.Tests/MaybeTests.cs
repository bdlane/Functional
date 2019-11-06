using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Functional.FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Functional.Tests
{
    public class MaybeTests
    {
        [Fact]
        public void BindReturnsEmptyWhenSourceIsEmpty()
        {
            // Arrange
            var sut = new None<Guid>();

            // Act
            var actual = sut.Bind(_ => Guid.NewGuid());

            // Assert
            actual.Match(true, _ => false).Should().BeTrue();
        }

        [Theory]
        [AutoData]
        public void BindReturnsResultOfSelectorWhenSourceIsFilled(int x, int y)
        {
            // Arrange
            var sut = new Some<int>(x);
            var expected = x + y;

            // Act
            var actual = sut.Bind(_ => x);

            // Assert
            actual.Match(0, i => i + y).Should().Be(expected);
        }

        // Match -> return Action
        // When -> executes Action

        [Theory]
        [AutoData]
        public void MatchExecutesNoneAction(int actual, int expected)
        {
            // Arrange
            var sut = new None<int>();

            // Act
            sut.Match(() => actual = expected, (v) => actual = v)();

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [AutoData]
        public void MatchExecutesSomeAction(int actual, int expected)
        {
            // Arrange
            var sut = new Some<int>(expected);

            // Act
            sut.Match(() => actual = 0, (v) => actual = v)();

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [AutoData]
        public void FromReturnsSomeWhenValueIsNotNull(object value)
        {
            // Arrange

            // Act
            var actual = Maybe.From(value);

            // Assert
            actual.Match(new object(), v => v).Should().Be(value);
        }

        [Fact]
        public void FromReturnsNoneWhenValueIsNull()
        {
            // Arrange

            // Act
            var actual = Maybe.From((object)null);

            // Assert
            actual.Should().BeEmpty();
        }

        [Theory]
        [AutoData]
        public void FromWithPredicateReturnsSomeWhenValueIsNotNullAndPredicateIsTrue(object value)
        {
            // Arrange

            // Act
            var actual = Maybe.From(value, v => v == value);

            // Assert
            actual.Match(new object(), v => v).Should().Be(value);
        }

        [Theory]
        [AutoData]
        public void FromWithPredicateReturnsNoneWhenValueIsNotNullAndPredicateIsFalse(object value)
        {
            // Arrange

            // Act
            var actual = Maybe.From(value, _ => false);

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public void FromWithPredicateReturnsNoneWhenValueIsNullAndPredicateIsTrue()
        {
            // Arrange

            // Act
            var actual = Maybe.From((object)null, _ => true);

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public void FromIsNullOrEmptyReturnsNoneWhenStringIsNull()
        {
            // Arrange

            // Act
            var actual = Maybe.FromIsNullOrEmpty(null);

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public void FromIsNullOrEmptyReturnsNoneWhenStringIsEmpty()
        {
            // Arrange

            // Act
            var actual = Maybe.FromIsNullOrEmpty("");

            // Assert
            actual.Should().BeEmpty();
        }

        [Theory]
        [AutoData]
        public void FromIsNullOrEmptyReturnsFilledWhenStringIsNotEmpty(string s)
        {
            // Arrange

            // Act
            var actual = Maybe.FromIsNullOrEmpty(s);

            // Assert
            actual.Should().BeSomething();
        }

        [Fact]
        public void EmptyReturnsNone()
        {
            // Arrange

            // Act

            // Assert
            Maybe.Empty<object>().Should().BeEmpty();
        }

        [Fact]
        public void ChooseReturnsCollectionWithOnlyFilledMaybes()
        {
            // Arrange
            var fixture = new Fixture();

            var expected = fixture.Create<List<Guid>>();
            var empty = fixture.Create<List<None<Guid>>>();

            var input = expected.Select(g => Maybe.From(g))
                                .Cast<IMaybe<Guid>>()
                                .Union(empty);

            // Act
            var actual = input.Choose();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
