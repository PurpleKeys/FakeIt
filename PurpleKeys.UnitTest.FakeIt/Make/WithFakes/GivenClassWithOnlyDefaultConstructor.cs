namespace PurpleKeys.UnitTest.FakeIt.Make.WithFakes;

using PurpleKeys.FakeIt;
using System.Diagnostics.CodeAnalysis;

public class GivenClassWithOnlyDefaultConstructor
{
    [Fact]
    public void InstanceIsCreated()
    {
        var result = Make.WithFakes<MakeThis>();

        Assert.NotNull(result);
    }

    [Fact]
    public void ParameterIsProvided_InvalidOperationExceptionIsThrown()
    {
        var args = new Dictionary<string, object?>
        {
            { "parameter", new object() }
        };

        Assert.Throws<InvalidOperationException>(() =>
            Make.WithFakes<MakeThis>(args));
    }

    [ExcludeFromCodeCoverage]
    public class MakeThis
    {
    }
}