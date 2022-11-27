namespace PurpleKeys.UnitTest.FakeIt.Call.WithFakes;

using PurpleKeys.FakeIt;

public class CallStaticAction : CallActionsTests
{
    [Theory]
    [InlineData(nameof(CallMe.StaticAction))]
    [InlineData(nameof(CallMe.StaticActionWithParameter))]
    public override void WithNoProvidedValues_IsInvoked(string methodName)
    {
        CallMe.StaticActionInvokes = 0;
        Call.WithFakes<CallMe>(methodName);
        Assert.Equal(1, CallMe.StaticActionInvokes);
    }

    [Theory]
    [InlineData(nameof(CallMe.NonPublicStaticAction))]
    [InlineData(nameof(CallMe.NonPublicStaticActionWithParameter))]
    public override void NonPublicWithNoProvidedValues_ThrowsFakeItDiscoveryException(string methodName)
    {
        Assert.Throws<FakeItDiscoveryException>(() => Call.WithFakes<CallMe>(methodName));
    }
    
    public override void WithParameter_CallsAction()
    {
        CallMe.StaticActionInvokes = 0;
        var parameters = new
        {
            argument = "Text"
        };

        Call.WithFakes<CallMe>(nameof(CallMe.StaticActionWithParameter), parameters);

        Assert.Equal(1, CallMe.StaticActionInvokes);
        Assert.Equal("Text", CallMe.StaticArgument);
    }
    
    public override void NoActionCanBeFound_ThrowsFakeItDiscoveryException()
    {
        Assert.Throws<FakeItDiscoveryException>(() => Call.WithFakes<CallMe>("MissingAction"));
    }
    
    public override void TooManyActionsFound_ThrowsException()
    {
        Assert.ThrowsAny<Exception>(() => Call.WithFakes<CallMe>(nameof(CallMe.StaticOverloadedAction)));
    }
    
    public override void WithInvalidParameterNames_ThrowsFakeItDiscoveryException()
    {
        var args = new 
        {
            text = 123
        };
        
        Assert.Throws<FakeItDiscoveryException>(
            () => Call.WithFakes<CallMe>(nameof(CallMe.ActionWithParameter), args));
    }

    public override void WithInvalidParameterType_ThrowsFakeItDiscoveryException()
    {
        var args = new 
        {
            text = 123
        };
        
        Assert.Throws<FakeItDiscoveryException>(
            () => Call.WithFakes<CallMe>(nameof(CallMe.StaticActionWithParameter), args));
    }

    public override void TooManyOverloadedActionsFound_ThrowsException()
    {
        var args = new 
        {
            argument1 = "text"
        };
        
        Assert.Throws<FakeItDiscoveryException>(
            () => Call.WithFakes<CallMe>(nameof(CallMe.StaticParameterizedOverloadedAction), args));
    }
}
