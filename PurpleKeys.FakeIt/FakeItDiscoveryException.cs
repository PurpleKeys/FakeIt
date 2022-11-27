using System.Diagnostics.CodeAnalysis;

namespace PurpleKeys.FakeIt
{
    [ExcludeFromCodeCoverage]
    public class FakeItDiscoveryException : Exception
    {
        public Type TargetType { get; }
        public string TargetMethod { get; }
        public string Signature { get; }

        public FakeItDiscoveryException(string message, Type targetType, string targetMethod, string signature)
        : base(message)
        {
            TargetType = targetType;
            TargetMethod = targetMethod;
            Signature = signature;
        }

        public static FakeItDiscoveryException CreateStatic(
            string message, 
            Type targetType, 
            string targetMethod, 
            IReadOnlyDictionary<string,  object?>? parameters = null)
        {
            var sig = $"static {targetMethod}({string.Join(",", parameters?.Keys ?? Enumerable.Empty<string>())})";

            return new FakeItDiscoveryException(message, targetType, targetMethod, sig);
        }

        public static FakeItDiscoveryException CreateInstance(
            string message, 
            Type targetType, 
            string targetMethod, 
            IReadOnlyDictionary<string, object?>? parameters = null)
        {
            var sig = $"{targetMethod}({string.Join(",", parameters?.Keys ?? Enumerable.Empty<string>())})";

            return new FakeItDiscoveryException(message, targetType, targetMethod, sig);
        }
    }
}
