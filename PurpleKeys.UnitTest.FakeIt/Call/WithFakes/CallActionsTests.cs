namespace PurpleKeys.UnitTest.FakeIt.Call.WithFakes;

using System.Diagnostics.CodeAnalysis;

public abstract class CallActionsTests
{
    public abstract void NonPublicWithNoProvidedValues_ThrowsFakeItDiscoveryException(string methodName);
    [Fact]
    public abstract void WithInvalidParameterNames_ThrowsFakeItDiscoveryException();
    [Fact]
    public abstract void WithInvalidParameterType_ThrowsFakeItDiscoveryException();
    public abstract void WithNoProvidedValues_IsInvoked(string methodName);
    [Fact]
    public abstract void WithParameter_CallsAction();
    [Fact]
    public abstract void TooManyActionsFound_ThrowsException();
    [Fact]
    public abstract void TooManyOverloadedActionsFound_ThrowsException();
    [Fact]
    public abstract void NoActionCanBeFound_ThrowsFakeItDiscoveryException();

    [ExcludeFromCodeCoverage]
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

        internal void NonPublicAction()
        {
        }

        public void ActionWithParameter(string argument)
        {
            Interlocked.Increment(ref _actionInvokes);
            Argument = argument;
        }

        internal void NonPublicActionWithParameter(string argument)
        {
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

        internal static void NonPublicStaticAction()
        {
            Interlocked.Increment(ref _staticActionInvokes);
        }

        internal static void NonPublicStaticActionWithParameter(string argument)
        {
            Interlocked.Increment(ref _staticActionInvokes);
            StaticArgument = argument;
        }

        public void ParameterizedOverloadedAction(string argument1, string argument2)
        {
        }
        public void ParameterizedOverloadedAction(string argument1, int argument2)
        {
        }

        public static void StaticOverloadedAction()
        {
        }

        public static void StaticOverloadedAction(string argument)
        {
        }

        public static void StaticParameterizedOverloadedAction(string argument1, string argument2)
        {
        }
        public static void StaticParameterizedOverloadedAction(string argument1, int argument2)
        {
        }
    }
}
