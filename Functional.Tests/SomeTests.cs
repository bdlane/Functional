using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Theory, AutoData]
        public void TwoInstancesAsObjectsAreEqualWhenTheirContainedValuesAreEqual(Guid value)
        {
            // Arrange
            object m1 = new Some<Guid>(value);
            object m2 = new Some<Guid>(value);

            // Act

            // Assert
            m1.Equals(m2).Should().BeTrue();
        }

        [Theory, AutoData]
        public void ImplementsIEquatable(Some<Guid> some)
        {
            // Arrange

            // Act

            // Assert
            some.Should().BeAssignableTo<IEquatable<Some<Guid>>>();
        }

        [Theory, AutoData]
        public void TwoInstancesAreEqualWhenTheirContainedValuesAreEqual(Guid value)
        {
            // Arrange
            Some<Guid> m1 = new Some<Guid>(value);
            Some<Guid> m2 = new Some<Guid>(value);

            // Act

            // Assert
            m1.Equals(m2).Should().BeTrue();
        }

        [Theory, AutoData]
        public void TwoInstancesAreNotEqualWhenTheirContainedValuesAreNotEqual(Some<Guid> m1, Some<Guid> m2)
        {
            // Arrange

            // Act

            // Assert
            m1.Equals((object)m2).Should().BeFalse();
        }
    }
}
