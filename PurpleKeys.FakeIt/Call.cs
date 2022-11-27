using System.Reflection;
using PurpleKeys.FakeIt.InternalUse;

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
            
            RequiredSingleActionGuard<TTarget>(targetMethods, actionName, null, true);

            var parameters = targetMethods[0].GetParameters();
            var arguments = MockFactory.ParametersToArg(parameters);

            targetMethods[0].Invoke(target, arguments);
        }

        public static void WithFakes<TTarget>(string actionName)
        {
            actionName = actionName ?? throw new ArgumentNullException(nameof(actionName));

            var targetMethods = TargetMethods<TTarget>(actionName, BindingFlags.Static);
            
            RequiredSingleActionGuard<TTarget>(targetMethods, actionName, null, false);

            var parameters = targetMethods[0].GetParameters();
            var arguments = MockFactory.ParametersToArg(parameters);

            targetMethods[0].Invoke(null, arguments);
        }

        public static void WithFakes<TTarget>(
            string actionName,
            object specifiedParameters)
        {
            actionName = actionName ?? throw new ArgumentNullException(nameof(actionName));
            var specifiedParametersDictionary = ReflectionHelper.ObjectToDictionary(specifiedParameters);
            var methods = TargetMethods<TTarget>(actionName, BindingFlags.Static);

            RequiredSingleActionGuard<TTarget>(methods, actionName, specifiedParametersDictionary, false);

            if (!ReflectionHelper.TryParametersForMethod(methods, specifiedParametersDictionary, 
                    out var matchingMethod, out var matchingParameters, out var matchingErrorMessage))
            {
                throw FakeItDiscoveryException.CreateStatic(
                    matchingErrorMessage, 
                    typeof(TTarget), 
                    actionName,
                    specifiedParametersDictionary);
            }
            var arguments = MockFactory.ParametersToArg(matchingParameters!, specifiedParametersDictionary);

            matchingMethod!.Invoke(null, arguments);
        }

        public static void WithFakes<TTarget>(
            TTarget target, 
            string actionName,
            object specifiedParameters)
        {
            target = target ?? throw new ArgumentNullException(nameof(target));
            actionName = actionName ?? throw new ArgumentNullException(nameof(actionName));
            specifiedParameters = specifiedParameters ?? throw new ArgumentNullException(nameof(specifiedParameters));

            var specifiedParametersDictionary = ReflectionHelper.ObjectToDictionary(specifiedParameters);

            var methods = TargetMethods<TTarget>(actionName, BindingFlags.Instance);

            if (!ReflectionHelper.TryParametersForMethod(methods, specifiedParametersDictionary,
                    out var matchingMethod, out var matchingParameters, out var matchingErrorMessage))
            {
                throw FakeItDiscoveryException.CreateInstance(
                    matchingErrorMessage,
                    typeof(TTarget),
                    actionName,
                    specifiedParametersDictionary);
            }

            var arguments = MockFactory.ParametersToArg(matchingParameters!, specifiedParametersDictionary);

            matchingMethod!.Invoke(target, arguments);
        }

        #endregion

        #region Func
        public static TReturn? WithFakes<TTarget, TReturn>(TTarget target, string method)
        {
            var targetMethods = TargetMethods<TTarget, TReturn>(method, BindingFlags.Instance);

            RequiredSingleActionGuard<TTarget>(targetMethods, method, null, true);

            var parameters = targetMethods[0].GetParameters();
            var arguments = MockFactory.ParametersToArg(parameters);

            return (TReturn?)targetMethods[0].Invoke(target, arguments);
        }

        public static TReturn? WithFakes<TTarget, TReturn>(string method)
        {
            var targetMethods = TargetMethods<TTarget, TReturn>(method, BindingFlags.Static);

            RequiredSingleActionGuard<TTarget>(targetMethods, method, null, false);

            var parameters = targetMethods[0].GetParameters();
            var arguments = MockFactory.ParametersToArg(parameters);

            return (TReturn?)targetMethods[0].Invoke(null, arguments);
        }

        public static TReturn? WithFakes<TTarget, TReturn>(string functionName, object specifiedParameterValues)
        {
            var specifiedParametersDictionary = ReflectionHelper.ObjectToDictionary(specifiedParameterValues);
            var targetMethods = TargetMethods<TTarget, TReturn>(functionName, BindingFlags.Static);

            RequiredSingleActionGuard<TTarget>(targetMethods, functionName, specifiedParametersDictionary, false);

            if (!ReflectionHelper.TryParametersForMethod(targetMethods, specifiedParametersDictionary,
                    out var matchingMethod, out var matchingParameters, out var matchingErrorMessage))
            {
                throw FakeItDiscoveryException.CreateStatic(
                    matchingErrorMessage,
                    typeof(TTarget),
                    functionName,
                    specifiedParametersDictionary);
            }

            var arguments = MockFactory.ParametersToArg(matchingParameters!, specifiedParametersDictionary);

            return (TReturn?)matchingMethod!.Invoke(null, arguments);
        }

        public static TReturn? WithFakes<TTarget, TReturn>(TTarget target, string functionName, object specifiedParameterValues)
        {
            var specifiedParametersDictionary = ReflectionHelper.ObjectToDictionary(specifiedParameterValues);
            var targetMethods = TargetMethods<TTarget, TReturn>(functionName, BindingFlags.Instance);

            RequiredSingleActionGuard<TTarget>(targetMethods, functionName, specifiedParametersDictionary, true);

            if (!ReflectionHelper.TryParametersForMethod(targetMethods, specifiedParametersDictionary,
                    out var matchingMethod, out var matchingParameters, out var matchingErrorMessage))
            {
                throw FakeItDiscoveryException.CreateInstance(
                    matchingErrorMessage,
                    typeof(TTarget),
                    functionName,
                    specifiedParametersDictionary);
            }
            var arguments = MockFactory.ParametersToArg(matchingParameters!, specifiedParametersDictionary);

            return (TReturn?)matchingMethod!.Invoke(target, arguments);
        }

        #endregion

        private static MethodInfo[] TargetMethods<TTarget>(string method, BindingFlags staticOrInstance)
        {
            return typeof(TTarget)
                .GetMethods(BindingFlags.Public | staticOrInstance)
                .Where(m => m.Name == method).ToArray();
        }

        private static MethodInfo[] TargetMethods<TTarget, TReturn>(string method, BindingFlags staticOrInstance)
        {
            return typeof(TTarget)
                .GetMethods(BindingFlags.Public | staticOrInstance)
                .Where(m => m.Name == method && 
                            m.ReturnType != typeof(void) && 
                            typeof(TReturn).IsAssignableFrom(m.ReturnType)
                            ).ToArray();
        }

        private static void RequiredSingleActionGuard<T>(
            IReadOnlyCollection<MethodInfo> targetMethods,
            string method, 
            IReadOnlyDictionary<string, object?>? parameters, 
            bool isInstance)
        {
            if (targetMethods.Count == 1)
            {
                return;
            }

            var message = targetMethods.Count == 0
                ? "Can not Fake It when method can not be found"
                : "Can not Fake It when more than one method is found";

            throw isInstance
                ? FakeItDiscoveryException.CreateInstance(message, typeof(T), method, parameters)
                : FakeItDiscoveryException.CreateStatic(message, typeof(T), method, parameters);
        }
    }
}