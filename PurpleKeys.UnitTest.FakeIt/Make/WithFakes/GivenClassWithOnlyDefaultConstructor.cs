namespace PurpleKeys.UnitTest.FakeIt.Fill.WithFakes;
using PurpleKeys.FakeIt;

public class GivenClassWithOnlyDefaultConstructor
{
    [Fact]
    public void InstanceIsCreated()
    {
        var result = Fill.WithFakes<MakeThis>();

        Assert.NotNull(result);
    }

    [Fact]
    public void ParameterIsProvided_InvalidOperationExceptionIsThrown()
    {
        Assert.Throws<InvalidOperationException>(() =>
            Fill.WithFakes<MakeThis>(new Dictionary<string, object?>
            {
                { "parameter", new object() }
            }));
    }

    public class MakeThis { }
}