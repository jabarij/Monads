namespace Monads.FluentAssertions
{
    public static class ResultAssertionExtensions
    {
        public static ResultAssertions<TOk, TError> Should<TOk, TError>(this Result<TOk, TError> actualValue) => new ResultAssertions<TOk, TError>(actualValue);
        public static NullableResultAssertions<TOk, TError> Should<TOk, TError>(this Result<TOk, TError>? actualValue) => new NullableResultAssertions<TOk, TError>(actualValue);
    }
}
