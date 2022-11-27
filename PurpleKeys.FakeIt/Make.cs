using System.Reflection;
using PurpleKeys.FakeIt.InternalUse;

namespace PurpleKeys.FakeIt
{
    public static class Make
    {
        public static T WithFakes<T>()
        {
            var constructors = PublicConstructors<T>();
            if (constructors.Length != 1)
            {
                throw FakeItDiscoveryException.CreateInstance(
                    "Can not Fake It when a constructor is not available or has multiple public constructors.",
                    typeof(T),
                    "constructor");
            }

            var arguments = MockFactory.ParametersToArg(constructors[0].GetParameters());
            return (T)((ConstructorInfo)constructors[0]).Invoke(arguments);
        }

        public static T WithFakes<T>(object withDependencies)
        {
            var specifiedDependencyDictionary = ReflectionHelper.ObjectToDictionary(withDependencies);
            var constructors = PublicConstructors<T>();
            if (!ReflectionHelper.TryParametersForMethod(constructors, specifiedDependencyDictionary,
                    out var matchingConstructor, out var matchingParameters, out var matchingErrorMessage))
            {
                throw FakeItDiscoveryException.CreateStatic(
                    matchingErrorMessage,
                    typeof(T),
                    "Constructor",
                    specifiedDependencyDictionary);
            }
            var arguments = MockFactory.ParametersToArg(matchingParameters!, specifiedDependencyDictionary);

            return (T)((ConstructorInfo)matchingConstructor!).Invoke(arguments);
        }

        private static MethodBase[] PublicConstructors<T>()
        {
            return typeof(T).GetConstructors(BindingFlags.Public | BindingFlags.Instance).Cast<MethodBase>().ToArray();
        }
    }
}