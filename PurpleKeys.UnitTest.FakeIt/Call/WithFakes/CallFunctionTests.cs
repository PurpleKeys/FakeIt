namespace PurpleKeys.UnitTest.FakeIt.Call.WithFakes
{
    using System.Diagnostics.CodeAnalysis;

    public abstract class CallFunctionTests
    {
        public abstract void WithNoProvidedValues_ReturnsValue(string methodName, string expectedReturn);
        [Fact]
        public abstract void WithProvidedValues_ReturnsValue();
        public abstract void WithInvalidParameterNames_ThrowsFakeItDiscoveryException(string methodName);
        public abstract void NonPublicWithNoProvidedValues_ThrowsFakeItDiscoveryException(string methodName);
        [Fact] 
        public abstract void WithInvalidParameterType_ThrowsFakeItDiscoveryException();

        [ExcludeFromCodeCoverage]
        public class CallMe
        {
            public string Func() => "FuncValue";
            public string FuncWithParameter(string text) => text;

            internal string NonPublicFunc() => "FuncValue";
            internal string NonPublicFuncWithParameter(string text) => text;

            public static string StaticFunc() => "StaticFuncValue";
            public static string StaticFuncWithParameter(string text) => text;

            internal static string NonPublicStaticFunc() => "StaticFuncValue";
            internal static string NonPublicStaticFuncWithParameter(string text) => text;
        }
    }
}