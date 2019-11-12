using AutoFixture.Xunit2;
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

        [Fact]
        public void TwoNonesAsObjectsAreEqual()
        {
            // Arrange
            object m1 = new None<Guid>();
            object m2 = new None<Guid>();

            // Act

            // Assert
            m1.Equals(m2).Should().BeTrue();
        }

        [Fact]
        public void NullAndNoneAreNotEqual()
        {
            // Arrange
            object m1 = new None<Guid>();

            // Act

            // Assert
            m1.Equals(null).Should().BeFalse();
        }

        [Theory, AutoData]
        public void ImplementsIEquatable(None<Guid> none)
        {
            // Arrange

            // Act

            // Assert
            none.Should().BeAssignableTo<IEquatable<None<Guid>>>();
        }

        [Fact]
        public void TwoNonesAreEqual()
        {
            // Arrange
            None<Guid> m1 = new None<Guid>();
            None<Guid> m2 = new None<Guid>();

            // Act

            // Assert
            m1.Equals(m2).Should().BeTrue();
        }

        [Fact]
        public void TwoNonesOfDifferentTypesAreNotEqual()
        {
            // Arrange
            var m1 = new None<object>();
            var m2 = new None<Guid>();

            // Act

            // Assert
            m1.Equals((object)m2).Should().BeFalse();
        }
    }
}
