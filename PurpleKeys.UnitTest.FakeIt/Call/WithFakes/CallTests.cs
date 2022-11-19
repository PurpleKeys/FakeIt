namespace PurpleKeys.UnitTest.FakeIt.Call.WithFakes;

using PurpleKeys.FakeIt;

public class CallTests
{
    [Theory]
    [InlineData("Action")]
    [InlineData("ActionWithParameter")]
    public void ActionWithNoProvidedValues_IsInvoked(string methodName)
    {
        var target = new CallMe();
        Call.WithFakes(target, methodName);
        Assert.Equal(1, target.ActionInvokes);
    }

    [Theory]
    [InlineData("StaticAction")]
    [InlineData("StaticActionWithParameter")]
    public void StaticActionWithNoProvidedValues_IsInvoked(string methodName)
    {
        CallMe.StaticActionInvokes = 0;
        Call.WithFakes<CallMe>(methodName);
        Assert.Equal(1, CallMe.StaticActionInvokes);
    }

    [Fact]
    public void ActionWithUnrecognisedParameter_ThrowInvalidOperationException()
    {
        var parameters = new Dictionary<string, object?>
        {
            { "parameter", "text" }
        };
        Assert.Throws<InvalidOperationException>(() =>
            Call.WithFakes(this, nameof(CallMe.ActionWithParameter), parameters));
    }

    [Fact]
    public void ActionWithIncorrectParameterType_ThrowInvalidOperationException()
    {
        var parameters = new Dictionary<string, object?>
        {
            { "argument", 123 }
        };
        var target = new CallMe();
        Assert.Throws<InvalidOperationException>(() =>
            Call.WithFakes(target, nameof(CallMe.ActionWithParameter), parameters));
    }

    [Fact]
    public void ActionWithParameter_CallsAction()
    {
        var parameters = new Dictionary<string, object?>
        {
            { "argument", "Text" }
        };

        var target = new CallMe();

        Call.WithFakes(target, nameof(CallMe.ActionWithParameter), parameters);

        Assert.Equal(1, target.ActionInvokes);
        Assert.Equal("Text", target.Argument);
    }

    public class CallMe
    {
        private int _actionInvokes;
        public int ActionInvokes => _actionInvokes;

        private static int _staticActionInvokes;

        public static int StaticActionInvokes
        {
            get => _staticActionInvokes;
            set => _staticActionInvokes = value;
        }

        public string? Argument { get; set; }

        public static string? StaticArgument { get; set; }

        public void Action()
        {
            Interlocked.Increment(ref _actionInvokes);
        }

        public void ActionWithParameter(string argument)
        {
            Interlocked.Increment(ref _actionInvokes);
            Argument = argument;
        }

        public static void StaticAction()
        {
            Interlocked.Increment(ref _staticActionInvokes);
        }

        public static void StaticActionWithParameter(string argument)
        {
            Interlocked.Increment(ref _staticActionInvokes);
            StaticArgument = argument;
        }
    }
}