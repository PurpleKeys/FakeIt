using PurpleKeys.FakeIt.Internal;
using System.Reflection;

namespace PurpleKeys.FakeIt
{
    public static class Call
    {
        public static void WithFakes(object target, string method)
        {
            target = target ?? throw new ArgumentNullException(nameof(target));
            method = method ?? throw new ArgumentNullException(nameof(method));

            var targetMethods = target.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => m.Name == method).ToArray();

            RequiredSingleActionGuard(targetMethods);

            var parameters = targetMethods[0].GetParameters();
            var arguments = MockFactory.ParametersToArg(parameters);

            targetMethods[0].Invoke(target, arguments);
        }

        public static void WithFakes<T>(string method)
        {
            method = method ?? throw new ArgumentNullException(nameof(method));

            var targetMethods = typeof(T)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(m => m.Name == method).ToArray();

            RequiredSingleActionGuard(targetMethods);

            var parameters = targetMethods[0].GetParameters();
            var arguments = MockFactory.ParametersToArg(parameters);

            targetMethods[0].Invoke(null, arguments);
        }

        public static void WithFakes(object target, string actionName,
            Dictionary<string, object?> specifiedParameterValues)
        {
            target = target ?? throw new ArgumentNullException(nameof(target));
            actionName = actionName ?? throw new ArgumentNullException(nameof(actionName));
            specifiedParameterValues = specifiedParameterValues ??
                                       throw new ArgumentNullException(nameof(specifiedParameterValues));

            var methods = target.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.Name == actionName)
                .ToArray();

            var (method, parameters) = ReflectionHelper.ParametersForMethod(methods, specifiedParameterValues);
            var arguments = MockFactory.ParametersToArg(parameters, specifiedParameterValues);

            method.Invoke(target, arguments);
        }

        private static void RequiredSingleActionGuard(IReadOnlyCollection<MethodInfo> targetMethods)
        {
            if (targetMethods.Count == 0)
            {
                throw new ArgumentException("Can not Fake It when method can not be found");
            }

            if (targetMethods.Count > 1)
            {
                throw new ArgumentException("Can not Fake It when more than one method is found");
            }
        }
    }
}