using System.Diagnostics.CodeAnalysis;

namespace PurpleKeys.UnitTest.FakeIt.Make.WithFakes;

using PurpleKeys.FakeIt;
public class GivenClassWithMultipleConstructorsIncludingDefault
{
    [Fact]
    public void GivenNoParametersIsAmbiguous_ThrowsFakeItDiscoveryException()
    {
        Assert.Throws<FakeItDiscoveryException>(Make.WithFakes<MakeThis>);
    }

    [Fact]
    public void NonDefaultParameterIsProvided_InstanceIsReturned()
    {
        var result = Make.WithFakes<MakeThis>(new
        {
            parameter = "Text"
        });

        Assert.NotNull(result);
        Assert.Equal("Text", result.Parameter);
    }

    [Fact]
    public void PartialParametersAreProvided_ThrowsFakeItDiscoveryException()
    {
        Assert.Throws<FakeItDiscoveryException>(() => Make.WithFakes<MakeThis>(new 
        {
            parameter2 = 123 
        }));
    }

    [ExcludeFromCodeCoverage]
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

    [ExcludeFromCodeCoverage]
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