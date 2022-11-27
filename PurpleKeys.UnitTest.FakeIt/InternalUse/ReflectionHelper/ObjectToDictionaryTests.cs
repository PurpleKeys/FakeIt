
namespace PurpleKeys.UnitTest.FakeIt.InternalUse.ReflectionHelper
{
    using PurpleKeys.FakeIt.InternalUse;

    public class ObjectToDictionaryTests
    {
        [Fact]
        public void NullObject_ReturnsEmptyDictionary()
        {
            Assert.Empty(ReflectionHelper.ObjectToDictionary(null));
        }

        [Fact]
        public void AnonymousObject_ReturnsDictionaryOfProperties()
        {
            var result = ReflectionHelper.ObjectToDictionary(new
            {
                argument = "Value"
            });

            Assert.Equal("Value", result["argument"]);
        }

        [Fact]
        public void Dictionary_ReturnsTheSameDictionary()
        {
            var parameters = new Dictionary<string, object?>
            {
                { "Parameter", "Value" }
            };

            var result = ReflectionHelper.ObjectToDictionary(parameters);

            Assert.Same(parameters, result);

        }
    }
}
