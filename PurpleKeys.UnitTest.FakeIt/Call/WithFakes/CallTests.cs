namespace PurpleKeys.UnitTest.FakeIt.Call.WithFakes;

using PurpleKeys.FakeIt;
public class CallInstanceAction : CallActionsTests
{

    [Theory]
    [InlineData(nameof(CallMe.Action))]
    [InlineData(nameof(CallMe.ActionWithParameter))]
    public override void WithNoProvidedValues_IsInvoked(string methodName)
    {
        var target = new CallMe();
        Call.WithFakes(target, methodName);
        Assert.Equal(1, target.ActionInvokes);
    }

    [Theory]
    [InlineData(nameof(CallMe.NonPublicAction))]
    [InlineData(nameof(CallMe.NonPublicActionWithParameter))]
    public override void NonPublicWithNoProvidedValues_ThrowsFakeItDiscoveryException(string methodName)
    {
        var target = new CallMe();
        Assert.Throws<FakeItDiscoveryException>(() => Call.WithFakes(target, methodName));
    }
    
    public override void WithInvalidParameterNames_ThrowsFakeItDiscoveryException()
    {
        var parameters = new 
        {
            parameter = "text"
        };
        Assert.Throws<FakeItDiscoveryException>(() =>
            Call.WithFakes(this, nameof(CallMe.ActionWithParameter), parameters));
    }
    
    public override void WithInvalidParameterType_ThrowsFakeItDiscoveryException()
    {
        var args = new 
        {
            text = 123
        };

        var target = new CallMe();
        Assert.Throws<FakeItDiscoveryException>(
            () => Call.WithFakes<CallMe, string>(target, nameof(CallMe.ActionWithParameter), args));
    }
    
    public override void WithParameter_CallsAction()
    {
        var parameters = new 
        {
            argument = "Text"
        };

        var target = new CallMe();

        Call.WithFakes(target, nameof(CallMe.ActionWithParameter), parameters);

        Assert.Equal(1, target.ActionInvokes);
        Assert.Equal("Text", target.Argument);
    }

    public override void NoActionCanBeFound_ThrowsFakeItDiscoveryException()
    {
        var target = new CallMe();
        Assert.Throws<FakeItDiscoveryException>(() => Call.WithFakes(target, "MissingAction"));
    }
    
    public override void TooManyActionsFound_ThrowsException()
    {
        var target = new CallMe();
        Assert.ThrowsAny<Exception>(() => Call.WithFakes(target, nameof(CallMe.StaticOverloadedAction)));
    }

    public override void TooManyOverloadedActionsFound_ThrowsException()
    {
        var args = new 
        {
            argument1 = "text"
        };

        var target = new CallMe();
        Assert.Throws<FakeItDiscoveryException>(
            () => Call.WithFakes<CallMe, string>(target, nameof(CallMe.ParameterizedOverloadedAction), args));
    }
}