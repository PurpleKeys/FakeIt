namespace PurpleKeys.UnitTest.FakeIt.Make.WithFakes;
using PurpleKeys.FakeIt;

public class GivenClassWithOnlyOneDefaultConstructor
{
    [Fact]
    public void ParameterIsProvided_InstanceIsCreatedWithProvidedArgument()
    {
        var parameter = new Parameter();
        var args = new Dictionary<string, object?>
        {
            { "parameter", parameter }
        };

        var result = Make.WithFakes<MakeThis<Parameter>>(args);

        Assert.NotNull(result);
        Assert.Same(parameter, result.ParameterValue);
    }

    [Fact]
    public void UnknownParameterIsProvided_InvalidOperationExceptionIsThrown()
    {
        var args = new Dictionary<string, object?>
        {
            { "unknownParameter", new Parameter() }
        };

        Assert.Throws<InvalidOperationException>(() =>
            Make.WithFakes<MakeThis<Parameter>>(args));
    }

    [Fact]
    public void ParameterOfWrongTypeIsProvided_InvalidOperationExceptionIsThrown()
    {
        var args = new Dictionary<string, object?>
        {
            { "parameter", new object() }
        };

        Assert.Throws<InvalidOperationException>(() =>
            Make.WithFakes<MakeThis<Parameter>>(args));
    }

    [Fact]
    public void NullParameterIsProvided_InstanceIsCreatedWithProvidedArgument()
    {
        var result =Make.WithFakes<MakeThis<Parameter>>(new Dictionary<string, object?>
        {
            { "parameter", null }
        });

        Assert.Null(result.ParameterValue);
    }

    [Fact]
    public void NullableParameterIsProvided_InstanceIsCreatedWithProvidedArgument()
    {
        var result = Make.WithFakes<MakeThis<int?>>(new Dictionary<string, object?>
        {
            { "parameter", null }
        });

        Assert.Null(result.ParameterValue);
    }

    [Fact]
    public void NoParametersProvided_InstanceIsCreatedWithDefaultMocksForArguments()
    {
        var result = Make.WithFakes<MakeThis<Parameter>>();

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