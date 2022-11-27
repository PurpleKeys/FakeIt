using System.Diagnostics.CodeAnalysis;

namespace PurpleKeys.UnitTest.FakeIt.Make.WithFakes;

using PurpleKeys.FakeIt;

public class GivenTypeThatCanNotBeInstantiated
{
    [Fact]
    public void Interface_ThrowsFakeDiscoveryException()
    {
        Assert.Throws<FakeItDiscoveryException>(Make.WithFakes<IEqualityComparer<string>>);
    }

    [Fact]
    public void AbstractClass_ThrowsFakeDiscoveryException()
    {
        Assert.Throws<FakeItDiscoveryException>(Make.WithFakes<AbstractClass>);
    }

    [Fact]
    public void ClassWithNoPublicConstructor_ThrowsFakeDiscoveryException()
    {
        Assert.Throws<FakeItDiscoveryException>(Make.WithFakes<ClassWithNoPublicConstructor>);
    }

    [ExcludeFromCodeCoverage]
    public abstract class AbstractClass
    {
    }

    [ExcludeFromCodeCoverage]
    public class ClassWithNoPublicConstructor
    {
        internal ClassWithNoPublicConstructor()
        {
        }
    }
}
