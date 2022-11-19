namespace PurpleKeys.UnitTest.FakeIt.Fill.WithFakes;
using PurpleKeys.FakeIt;

public class GivenClassWithOnlyOneDefaultConstructor
{
    [Fact]
    public void ParameterIsProvided_InstanceIsCreatedWithProvidedArgument()
    {
        var parameter = new Parameter();
        var result = Fill.WithFakes<MakeThis<Parameter>>(new Dictionary<string, object?>
        {
            { "parameter", parameter }
        });

        Assert.NotNull(result);
        Assert.Same(parameter, result.ParameterValue);
    }

    [Fact]
    public void UnknownParameterIsProvided_InvalidOperationExceptionIsThrown()
    {
        Assert.Throws<InvalidOperationException>(() =>
            Fill.WithFakes<MakeThis<Parameter>>(new Dictionary<string, object?>
            {
                { "unknownParameter", new Parameter() }
            }));
    }

    [Fact]
    public void ParameterOfWrongTypeIsProvided_InvalidOperationExceptionIsThrown()
    {
        Assert.Throws<InvalidOperationException>(() =>
            Fill.WithFakes<MakeThis<Parameter>>(new Dictionary<string, object?>
            {
                { "parameter", new object() }
            }));
    }

    [Fact]
    public void NullParameterIsProvided_InstanceIsCreatedWithProvidedArgument()
    {
        var result = Fill.WithFakes<MakeThis<Parameter>>(new Dictionary<string, object?>
        {
            { "parameter", null }
        });

        Assert.Null(result.ParameterValue);
    }

    [Fact]
    public void NullableParameterIsProvided_InstanceIsCreatedWithProvidedArgument()
    {
        var result = Fill.WithFakes<MakeThis<int?>>(new Dictionary<string, object?>
        {
            { "parameter", null }
        });

        Assert.Null(result.ParameterValue);
    }

    [Fact]
    public void NoParametersProvided_InstanceIsCreatedWithDefaultMocksForArguments()
    {
        var result = Fill.WithFakes<MakeThis<Parameter>>();

        Assert.NotNull(result);
        Assert.NotNull(result.ParameterValue);
    }

    public class Parameter
    {
    }

    public class MakeThis<T>
    {
        public MakeThis(T parameter)
        {
            ParameterValue = parameter;
        }

        public T ParameterValue { get; }
    }
}
