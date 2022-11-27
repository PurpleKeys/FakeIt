

namespace PurpleKeys.UnitTest.FakeIt.Call.WithFakes
{
    using PurpleKeys.FakeIt;

    public class CallInstanceFunction : CallFunctionTests
    {
        [Theory]
        [InlineData(nameof(CallMe.Func), "FuncValue")]
        [InlineData(nameof(CallMe.FuncWithParameter), null)]
        public override void WithNoProvidedValues_ReturnsValue(string methodName, string expectedReturn)
        {
            var target = new CallMe();
            var result = Call.WithFakes<CallMe, string>(target, methodName);
            Assert.Equal(expectedReturn, result);
        }

        [Fact]
        public override void WithProvidedValues_ReturnsValue()
        {
            var args = new Dictionary<string, object?>
            {
                { "text", "TestValue" }
            };

            var target = new CallMe();
            var result = Call.WithFakes<CallMe, string>(target, nameof(CallMe.FuncWithParameter), args);

            Assert.Equal("TestValue", result);
        }

        [Theory]
        [InlineData(nameof(CallMe.Func))]
        [InlineData(nameof(CallMe.FuncWithParameter))]
        public override void WithInvalidParameterNames_ThrowsFakeItDiscoveryException(string methodName)
        {
            var args = new Dictionary<string, object?>
            {
                { "invalidName", "TestValue" }
            };

            var target = new CallMe();
            Assert.Throws<FakeItDiscoveryException>(() => Call.WithFakes<CallMe, string>(target, methodName, args));
        }
        
        [Theory]
        [InlineData(nameof(CallMe.NonPublicStaticFunc))]
        [InlineData(nameof(CallMe.NonPublicStaticFuncWithParameter))]
        public override void NonPublicWithNoProvidedValues_ThrowsFakeItDiscoveryException(string methodName)
        {
            Assert.Throws<FakeItDiscoveryException>(() => Call.WithFakes<CallMe>(methodName));
        }

        [Fact]
        public override void WithInvalidParameterType_ThrowsFakeItDiscoveryException()
        {
            var args = new Dictionary<string, object?>
            {
                { "text", 123 }
            };

            var target = new CallMe();
            Assert.Throws<FakeItDiscoveryException>(
                () => Call.WithFakes<CallMe, string>(target, nameof(CallMe.FuncWithParameter), args));
        }
    }
}