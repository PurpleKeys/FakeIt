using PurpleKeys.FakeIt.Internal;
using System.Reflection;

namespace PurpleKeys.FakeIt
{
    public static class Call
    {
        #region Actions
        public static void WithFakes<TTarget>(TTarget target, string actionName)
        {
            target = target ?? throw new ArgumentNullException(nameof(target));
            actionName = actionName ?? throw new ArgumentNullException(nameof(actionName));

            var targetMethods = TargetMethods<TTarget>(actionName, BindingFlags.Instance);

            RequiredSingleActionGuard(targetMethods);

            var parameters = targetMethods[0].GetParameters();
            var arguments = MockFactory.ParametersToArg(parameters);

            targetMethods[0].Invoke(target, arguments);
        }

        public static void WithFakes<TTarget>(string actionName)
        {
            actionName = actionName ?? throw new ArgumentNullException(nameof(actionName));

            var targetMethods = TargetMethods<TTarget>(actionName, BindingFlags.Static);

            RequiredSingleActionGuard(targetMethods);

            var parameters = targetMethods[0].GetParameters();
            var arguments = MockFactory.ParametersToArg(parameters);

            targetMethods[0].Invoke(null, arguments);
        }

        public static void WithFakes<TTarget>(
            string actionName, 
            Dictionary<string, object?> specifiedParameterValues)
        {
            actionName = actionName ?? throw new ArgumentNullException(nameof(actionName));

            var methods = TargetMethods<TTarget>(actionName, BindingFlags.Static);

            RequiredSingleActionGuard(methods);

            var (method, parameters) = ReflectionHelper.ParametersForMethod(methods, specifiedParameterValues);
            var arguments = MockFactory.ParametersToArg(parameters, specifiedParameterValues);

            method.Invoke(null, arguments);
        }

        public static void WithFakes<TTarget>(
            TTarget target, 
            string actionName,
            Dictionary<string, object?> specifiedParameterValues)
        {
            target = target ?? throw new ArgumentNullException(nameof(target));
            actionName = actionName ?? throw new ArgumentNullException(nameof(actionName));
            specifiedParameterValues = specifiedParameterValues ??
                                       throw new ArgumentNullException(nameof(specifiedParameterValues));

            var methods = TargetMethods<TTarget>(actionName, BindingFlags.Instance);

            var (method, parameters) = ReflectionHelper.ParametersForMethod(methods, specifiedParameterValues);
            var arguments = MockFactory.ParametersToArg(parameters, specifiedParameterValues);

            method.Invoke(target, arguments);
        }

        #endregion

        #region Func
        public static TReturn? WithFakes<TTarget, TReturn>(TTarget target, string method)
        {
            var targetMethods = TargetMethods<TTarget, TReturn>(method, BindingFlags.Instance);

            RequiredSingleActionGuard(targetMethods);

            var parameters = targetMethods[0].GetParameters();
            var arguments = MockFactory.ParametersToArg(parameters);

            return (TReturn?)targetMethods[0].Invoke(target, arguments);
        }

        public static TReturn? WithFakes<TTarget, TReturn>(string method)
        {
            var targetMethods = TargetMethods<TTarget, TReturn>(method, BindingFlags.Static);

            RequiredSingleActionGuard(targetMethods);

            var parameters = targetMethods[0].GetParameters();
            var arguments = MockFactory.ParametersToArg(parameters);

            return (TReturn?)targetMethods[0].Invoke(null, arguments);
        }

        public static TReturn? WithFakes<TTarget, TReturn>(string functionName, IReadOnlyDictionary<string, object?> specifiedParameterValues)
        {
            var targetMethods = TargetMethods<TTarget, TReturn>(functionName, BindingFlags.Static);

            RequiredSingleActionGuard(targetMethods);

            var (method, parameters) = ReflectionHelper.ParametersForMethod(targetMethods, specifiedParameterValues);
            var arguments = MockFactory.ParametersToArg(parameters, specifiedParameterValues);

            return (TReturn?)method.Invoke(null, arguments);
        }

        public static TReturn? WithFakes<TTarget, TReturn>(TTarget target, string functionName, IReadOnlyDictionary<string, object?> specifiedParameterValues)
        {
            var targetMethods = TargetMethods<TTarget, TReturn>(functionName, BindingFlags.Instance);

            RequiredSingleActionGuard(targetMethods);

            var (method, parameters) = ReflectionHelper.ParametersForMethod(targetMethods, specifiedParameterValues);
            var arguments = MockFactory.ParametersToArg(parameters, specifiedParameterValues);

            return (TReturn?)method.Invoke(target, arguments);
        }

        #endregion

        private static MethodInfo[] TargetMethods<TTarget>(string method, BindingFlags staticOrInstance)
        {
            method = method ?? throw new ArgumentNullException(nameof(method));
            
            return typeof(TTarget)
                .GetMethods(BindingFlags.Public | staticOrInstance)
                .Where(m => m.Name == method).ToArray();
        }

        private static MethodInfo[] TargetMethods<TTarget, TReturn>(string method, BindingFlags staticOrInstance)
        {
            method = method ?? throw new ArgumentNullException(nameof(method));

            return typeof(TTarget)
                .GetMethods(BindingFlags.Public | staticOrInstance)
                .Where(m => m.Name == method && 
                            m.ReturnType != typeof(void) && 
                            typeof(TReturn).IsAssignableFrom(m.ReturnType)
                            ).ToArray();
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