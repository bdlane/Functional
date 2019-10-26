using FluentAssertions;
using System;
using Xunit;

namespace Functional.Tests
{
    public class NothingTests
    {
        [Fact]
        public void MatchReturnsNothingArgument()
        {
            // Arrange
            var sut = new Nothing<Guid>();

            ;            // Act
            var actual = sut.Match("empty", _ => "not empty");

            // Assert
            actual.Should().Be("empty");
        }

        [Fact]
        public void NothingArgumentCannotBeNull()
        {
            // Arrange
            var sut = new Nothing<Guid>();

            // Act
            Action act = () => sut.Match(null, _ => "not empty");

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }


    }
}
