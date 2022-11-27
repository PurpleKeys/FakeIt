

namespace PurpleKeys.UnitTest.FakeIt.Call.WithFakes
{
    using PurpleKeys.FakeIt;

    public class CallStaticFunction: CallFunctionTests
    {
        [Theory]
        [InlineData(nameof(CallMe.StaticFunc), "StaticFuncValue")]
        [InlineData(nameof(CallMe.StaticFuncWithParameter), null)]
        public override void WithNoProvidedValues_ReturnsValue(string methodName, string expectedReturn)
        {
            var result = Call.WithFakes<CallMe, string>(methodName);
            Assert.Equal(expectedReturn, result);
        }

        [Fact]
        public override void WithProvidedValues_ReturnsValue()
        {
            var args = new 
            {
                text = "TestValue"
            };

            var result = Call.WithFakes<CallMe, string>(nameof(CallMe.StaticFuncWithParameter), args);

            Assert.Equal("TestValue", result);
        }

        [Theory]
        [InlineData(nameof(CallMe.StaticFunc))]
        [InlineData(nameof(CallMe.StaticFuncWithParameter))]
        public override void WithInvalidParameterNames_ThrowsFakeItDiscoveryException(string methodName)
        {
            var args = new
            {
                invalidName = "TestValue"
            };
            
            Assert.Throws<FakeItDiscoveryException>(
                () => Call.WithFakes<CallMe, string>(methodName, args));
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
            var args = new 
            {
                text = 123
            };
            
            Assert.Throws<FakeItDiscoveryException>(
                () => Call.WithFakes<CallMe, string>(nameof(CallMe.StaticFuncWithParameter), args));
        }
    }
}