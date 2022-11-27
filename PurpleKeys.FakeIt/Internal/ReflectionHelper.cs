using System.Reflection;

namespace PurpleKeys.FakeIt.Internal
{
    internal static class ReflectionHelper
    {
        public static bool MethodHasParametersForAllParameters(
            MethodBase method,
            IReadOnlyDictionary<string, object?> withDependencies)
        {
            var parameters = method.GetParameters();
            return withDependencies.Keys.All(k => MatchParametersAndArguments(withDependencies, k, parameters));
        }

        public static bool TryParametersForMethod(
            IReadOnlyList<MethodBase> methods,
            IReadOnlyDictionary<string, object?> specifiedParameterValues,
            out MethodBase? matchingMethod, 
            out ParameterInfo[]? matchingParameters,
            out string matchingErrorMessage)
        {
            var possibleOptions = methods
                .Where(c => MethodHasParametersForAllParameters(c, specifiedParameterValues))
                .ToArray();

            if (possibleOptions.Length != 1)
            {
                var message = possibleOptions.Length == 0
                    ? "Can not Fake It when no method can be found"
                    : "Can not Fake It when more than one method is found. Try requesting a more specific overload";

                matchingMethod = null;
                matchingParameters = null;
                matchingErrorMessage = message;
                return false;
            }

            matchingMethod = possibleOptions[0];
            matchingParameters = possibleOptions[0].GetParameters();
            matchingErrorMessage = string.Empty;
            return true;
        }

        private static bool MatchParametersAndArguments(
            IReadOnlyDictionary<string, object?> withDependencies,
            string argumentName, ParameterInfo[] parameters)
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