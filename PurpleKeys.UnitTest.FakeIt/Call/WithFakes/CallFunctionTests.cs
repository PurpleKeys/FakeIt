namespace PurpleKeys.UnitTest.FakeIt.Call.WithFakes
{
    public class CallFunctionTests
    {
        [Theory]
        [InlineData(nameof(CallMe.Func), "FuncValue")]
        [InlineData(nameof(CallMe.FuncWithParameter), null)]
        public void FunctionWithNoProvidedValues_ReturnsValue(string methodName, string expectedReturn)
        {
            var target = new CallMe();
            var result = PurpleKeys.FakeIt.Call.WithFakes<CallMe, string>(target, methodName);
            Assert.Equal(expectedReturn, result);
        }

        [Theory]
        [InlineData(nameof(CallMe.StaticFunc), "StaticFuncValue")]
        [InlineData(nameof(CallMe.StaticFuncWithParameter), null)]
        public void StaticFunctionWithNoProvidedValues_ReturnsValue(string methodName, string expectedReturn)
        {
            var result = PurpleKeys.FakeIt.Call.WithFakes<CallMe, string>(methodName);
            Assert.Equal(expectedReturn, result);
        }

        [Fact]
        public void StaticFunctionCalledWithProvidedValues_ReturnsValue()
        {
            var args = new Dictionary<string, object?>
            {
                { "text", "TestValue" }
            };

            var result = PurpleKeys.FakeIt.Call.WithFakes<CallMe, string>(nameof(CallMe.StaticFuncWithParameter), args);

            Assert.Equal("TestValue", result);
        }

        [Fact]
        public void FunctionCalledWithProvidedValues_ReturnsValue()
        {
            var args = new Dictionary<string, object?>
            {
                { "text", "TestValue" }
            };

            var target = new CallMe();
            var result = PurpleKeys.FakeIt.Call.WithFakes<CallMe, string>(target, nameof(CallMe.FuncWithParameter), args);

            Assert.Equal("TestValue", result);
        }

        public class CallMe
        {
            public string Func() => "FuncValue";
            public string FuncWithParameter(string text) => text;

            public static string StaticFunc() => "StaticFuncValue";
            public static string StaticFuncWithParameter(string text) => text;
        }
    }
}