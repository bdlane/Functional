using AutoFixture;
using FluentAssertions;
using System;
using Xunit;

namespace Functional.AutoFixture.Tests
{
    public class MaybeSpecimenBuilderTests
    {
        [Fact]
        public void CannotCreateAMaybeFixtureWithoutCustomSpecimenBuilder()
        {
            var fixture = new Fixture();

            Action actual = () => fixture.Create<IMaybe<object>>();

            actual.Should().Throw<ObjectCreationException>();
        }

        [Fact]
        public void CanCreateAMaybeFixtureWithCustomSpecimenBuilder()
        {
            var fixture = new Fixture();

            fixture.Customizations.Add(new MaybeSpecimenBuilder());

            var actual = fixture.Create<IMaybe<object>>();

            actual.Should().BeAssignableTo<Some<object>>();
        }
    }
}
