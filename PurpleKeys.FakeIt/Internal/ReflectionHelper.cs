using System.Reflection;

namespace PurpleKeys.FakeIt.Internal
{
    internal static class ReflectionHelper
    {
        public static bool MethodHasParametersForAllParameters(MethodBase method, Dictionary<string, object?> withDependencies)
        {
            var parameters = method.GetParameters();
            return withDependencies.Keys.All(k => MatchParametersAndArguments(withDependencies, k, parameters));
        }

        public static (MethodBase, ParameterInfo[]) ParametersForMethod(IReadOnlyList<MethodBase> methods, Dictionary<string, object?> specifiedParameterValues)
        {
            if (methods.Count == 1)
            {
                if (!MethodHasParametersForAllParameters(methods[0], specifiedParameterValues))
                {
                    throw new InvalidOperationException("Can not Fake It with dependencies not existing on the method.");
                }

                var parameters = methods[0].GetParameters();
                return (methods[0], parameters);
            }

            var possibleOptions = methods
                .Where(c => MethodHasParametersForAllParameters(c, specifiedParameterValues))
                .ToArray();

            if (possibleOptions.Length != 1)
            {
                throw new InvalidOperationException("Can not Fake It with dependencies not existing on the method.");
            }

            return (possibleOptions[0], possibleOptions[0].GetParameters());
        }

        private static bool MatchParametersAndArguments(IDictionary<string, object?> withDependencies, string argumentName, ParameterInfo[] parameters)
        {
            var parameter = parameters.FirstOrDefault(p => p.Name == argumentName);
            var value = withDependencies[argumentName];

            if (parameter == null)
            {
                return false;
            }

            if (value == null && parameter.ParameterType.IsClass)
            {
                return true;
            }

            if (value == null && parameter.ParameterType.IsValueType)
            {
                return Nullable.GetUnderlyingType(parameter.ParameterType) != null;
            }

            return parameter.ParameterType.IsInstanceOfType(value);
        }
    }
}