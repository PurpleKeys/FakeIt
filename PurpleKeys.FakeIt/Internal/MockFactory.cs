using Moq;
using System.Reflection;

namespace PurpleKeys.FakeIt.Internal
{
    internal static class MockFactory
    {
        private static readonly MethodInfo GetDefaultMethodInfo;

        static MockFactory()
        {
            GetDefaultMethodInfo =
                typeof(MockFactory).GetMethod(nameof(GetDefault), BindingFlags.Static | BindingFlags.NonPublic)!;
        }

        public static object? CreateMockOf(Type type)
        {
            if (type.IsSealed)
            {
                return GetDefaultMethodInfo.MakeGenericMethod(type).Invoke(null, null);
            }

            return typeof(Mock)
                .GetMethod("Of", 1, BindingFlags.Public | BindingFlags.Static, null, Array.Empty<Type>(), null)
                ?.MakeGenericMethod(type)
                .Invoke(null, Array.Empty<object>());
        }

        public static object? UseValueOrCreateMockOf(IReadOnlyDictionary<string, object?> withDependencies,
            ParameterInfo p)
        {
            return withDependencies.TryGetValue(p.Name ?? string.Empty, out var dependency)
                ? dependency
                : CreateMockOf(p.ParameterType);
        }

        public static object?[] ParametersToArg(
            ParameterInfo[] parameters,
            IReadOnlyDictionary<string, object?> withDependencies)
        {
            return parameters
                .Select(p => UseValueOrCreateMockOf(withDependencies, p))
                .ToArray();
        }

        public static object?[] ParametersToArg(ParameterInfo[] parameters)
        {
            return parameters
                .Select(p => CreateMockOf(p.ParameterType))
                .ToArray();
        }

        private static T? GetDefault<T>()
        {
            return default;
        }
    }
}