using FluentAssertions;
using System;
using Xunit;

namespace Functional.Tests
{
    public class NoneTests
    {
        [Fact]
        public void MatchReturnsNoneArgument()
        {
            // Arrange
            var sut = new None<Guid>();

            ;            // Act
            var actual = sut.Match("empty", _ => "not empty");

            // Assert
            actual.Should().Be("empty");
        }

        [Fact]
        public void NoneArgumentCannotBeNull()
        {
            // Arrange
            var sut = new None<Guid>();

            // Act
            Action act = () => sut.Match(null, _ => "not empty");

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
