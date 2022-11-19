namespace PurpleKeys.UnitTest.FakeIt.Make.WithFakes;
using PurpleKeys.FakeIt;
public class GivenClassWithMultipleConstructorsIncludingDefault
{
    [Fact]
    public void GivenNoParameters_InvalidOperationExceptionIsThrown()
    {
        Assert.Throws<InvalidOperationException>(Make.WithFakes<MakeThis>);
    }

    [Fact]
    public void NonDefaultParameterIsProvided_InstanceIsReturned()
    {
        var result = Make.WithFakes<MakeThis>(new Dictionary<string, object?>
        {
            { "parameter", "Text" }
        });

        Assert.NotNull(result);
        Assert.Equal("Text", result.Parameter);
    }

    [Fact]
    public void PartialParametersAreProvided_InvalidOperationExceptionIsThrown()
    {
        Assert.Throws<InvalidOperationException>(() => Make.WithFakes<MakeThis>(new Dictionary<string, object?>
        {
            { "parameter2", 123 }
        }));
    }

    public class MakeThis
    {
        public string? Parameter { get; }

        public MakeThis()
        {
        }

        public MakeThis(string parameter)
        {
            Parameter = parameter;
        }
    }

    public class MakeThisWithMoreOverloads
    {
        public string? Parameter { get; }
        public int Parameter2 { get; }

        public MakeThisWithMoreOverloads()
        {
        }

        public MakeThisWithMoreOverloads(string parameter)
        {
            Parameter = parameter;
        }

        public MakeThisWithMoreOverloads(string parameter, int parameter2)
        {
            Parameter = parameter;
            Parameter2 = parameter2;
        }
    }
}