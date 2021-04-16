namespace Monads.DataOps.FluentAssertions
{
    public static class ClearableValueAssertionExtensions
    {
        public static ClearableValueAssertions<T> Should<T>(this ClearableValue<T> actualValue) => new ClearableValueAssertions<T>(actualValue);
        public static NullableClearableValueAssertions<T> Should<T>(this ClearableValue<T>? actualValue) => new NullableClearableValueAssertions<T>(actualValue);
    }
}
